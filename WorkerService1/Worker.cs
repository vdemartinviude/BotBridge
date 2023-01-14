using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Appccelerate.StateMachine.Machine.Reports;
using CiaExemplo.CiaDomain;
using CiaExemplo.PagesStates;
using JsonDocumentsManager;
using StatesAndEvents;
using System.Reflection;
using System.Text.Json;
using TheRobot;
using Serilog;

namespace WorkerService1;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Robot _robot;
    private ActiveStateMachine<BaseState, RobotEvents> _activeStateMachine;
    private WatchDog _watchDog;
    private readonly ResultJsonDocument _resultJsonDocument;
    private CancellationToken _cancellationToken;

    public Worker(ILogger<Worker> logger, Robot robot, BaseOrcamento baseOrcamento, WatchDog watchDog, ResultJsonDocument resultJsonDocument)
    {
        _resultJsonDocument = resultJsonDocument;
        _logger = logger;
        _robot = robot;

        Log.Information("Starting building the state machine");
        StateMachineDefinitionBuilder<BaseState, RobotEvents> builder = new();

        var PagesAssembly = Assembly.Load("CiaExemplo");

        List<BaseState> states = new();
        var statesToAdd = PagesAssembly.ExportedTypes.Where(x => x.BaseType == typeof(BaseState))
        .Select(x => (BaseState)Activator.CreateInstance(x, new object[] { _robot, baseOrcamento, _resultJsonDocument }));
        Log.Information("Events names: {@Names}", statesToAdd.Select((x, y) => y.ToString("00") + "->" + x.Name));
        states.AddRange(statesToAdd);

        foreach (var state in states)
        {
            builder.In(state).ExecuteOnEntry(() => state.MainExecute(_activeStateMachine));
            var Guards = PagesAssembly.GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGuard<,>)))
                .Select(x => new
                {
                    type = x,
                    currentstate = x.GetInterfaces().Single(y => y.GetGenericTypeDefinition() == typeof(IGuard<,>)).GenericTypeArguments[0],
                    nextstate = x.GetInterfaces().Single(y => y.GetGenericTypeDefinition() == typeof(IGuard<,>)).GenericTypeArguments[1],
                    theguard = Activator.CreateInstance(x)
                })
                .Where(x => x.currentstate == state.GetType());

            if (Guards.Any())
            {
                Log.Information($"For state {state.Name} we have the following transition guards:");
            }
            foreach (var guard in Guards.OrderBy(x => x.type.GetProperty("Priority").GetValue(x.theguard)))
            {
                Log.Information("\t{currentstateName} -> {nextstateName} with guard: {@guard}", guard.currentstate.Name, guard.nextstate.Name, guard);
                builder.In(state).On(RobotEvents.NormalTransition).If(() => (bool)guard.type.GetMethod("Condition").Invoke(guard.theguard, new object[] { _robot })).Goto(states.Single(x => x.GetType() == guard.nextstate));
            }

            var FinalGuards = PagesAssembly.GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGuard<>)))
                .Select(x => new
                {
                    type = x,
                    currentstate = x.GetInterfaces().Single(y => y.GetGenericTypeDefinition() == typeof(IGuard<>)).GenericTypeArguments[0]
                })
                .Where(x => x.currentstate == state.GetType());

            if (FinalGuards.Any())
            {
                Log.Information($"For state {state.Name} we have the following final guards:");
            }
            foreach (var guard in FinalGuards)
            {
                Log.Information("\t{currentstateName} -> FinalState with guard: {guard}", state.Name, guard);
                var theguard = Activator.CreateInstance(guard.type);
                builder
                    .In(state).On(RobotEvents.NormalTransition)
                    .If(() => (bool)guard.type.GetMethod("Condition")
                    .Invoke(theguard, new object[] { _robot }))
                    //.Goto(state)
                    .Execute(() => _finalizaMaquina());
            }
        }

        builder.WithInitialState(states.Single(x => x.GetType() == typeof(FirstPage)));
        _activeStateMachine = builder.Build().CreateActiveStateMachine();
        _watchDog = watchDog;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _cancellationToken = stoppingToken;

        var generator = new StateMachineReportGenerator<BaseState, RobotEvents>();
        _activeStateMachine.Report(generator);

        string report = generator.Result;
        _logger.LogInformation(report);

        _activeStateMachine.TransitionCompleted += _activeStateMachine_TransitionCompleted;
        _activeStateMachine.TransitionExceptionThrown += _activeStateMachine_TransitionExceptionThrown;
        try
        {
            _activeStateMachine.Start();
        }
        catch (Appccelerate.StateMachine.Machine.StateMachineException ex)
        {
            _logger.LogCritical(ex.InnerException.Message);
        }
        while (!stoppingToken.IsCancellationRequested && _activeStateMachine.IsRunning)
        {
            _logger.LogInformation("State Machine Working at: {time}", DateTimeOffset.Now);
            await Task.Delay(500, stoppingToken);
        }
        await _resultJsonDocument.SaveDocument("FinalResult.json");
    }

    private void _activeStateMachine_TransitionExceptionThrown(object sender, Appccelerate.StateMachine.Machine.Events.TransitionExceptionEventArgs<BaseState, RobotEvents> e)
    {
        _logger.LogCritical("EXCEPTION!!!!");
        _activeStateMachine.Stop();
    }

    private void _activeStateMachine_TransitionCompleted(object sender, Appccelerate.StateMachine.Machine.Events.TransitionCompletedEventArgs<BaseState, RobotEvents> e)
    {
        _logger.LogWarning("TRANSIÇÃO REALIZADA!!!!");
        _watchDog.SetWatchDog();
    }

    private void _finalizaMaquina()
    {
        _robot.Dispose();
        _resultJsonDocument.SaveDocument("FinalResult.json").Wait();
        _activeStateMachine.Stop();
    }
}