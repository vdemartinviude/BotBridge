using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Appccelerate.StateMachine.Machine.Reports;
using CiaExemplo.CiaDomain;
using CiaExemplo.PagesStates;
using StatesAndEvents;
using System.Reflection;
using System.Text.Json;
using TheRobot;

namespace WorkerService1;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Robot _robot;
    private ActiveStateMachine<BaseState, RobotEvents> _activeStateMachine;
    private WatchDog _watchDog;

    public Worker(ILogger<Worker> logger, Robot robot, BaseOrcamento baseOrcamento, WatchDog watchDog)
    {
        _logger = logger;
        _robot = robot;
        StateMachineDefinitionBuilder<BaseState, RobotEvents> builder = new();

        var PagesAssembly = Assembly.Load("CiaExemplo");

        List<BaseState> states = new();
        states.AddRange(PagesAssembly.ExportedTypes.Where(x => x.BaseType == typeof(BaseState))
        .Select(x => (BaseState)Activator.CreateInstance(x, new object[] { _robot, baseOrcamento })));

        foreach (var state in states)
        {
            builder.In(state).ExecuteOnEntry(() => state.MainExecute(_activeStateMachine));
            var Guards = PagesAssembly.GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGuard<,>)))
                .Select(x => new
                {
                    type = x,
                    currentstate = x.GetInterfaces().Single(y => y.GetGenericTypeDefinition() == typeof(IGuard<,>)).GenericTypeArguments[0],
                    nextstate = x.GetInterfaces().Single(y => y.GetGenericTypeDefinition() == typeof(IGuard<,>)).GenericTypeArguments[1]
                })
                .Where(x => x.currentstate == state.GetType());
            foreach (var guard in Guards)
            {
                var theguard = Activator.CreateInstance(guard.type);
                builder.In(state).On(RobotEvents.NormalTransition).If(() => (bool)guard.type.GetMethod("Condition").Invoke(theguard, new object[] { _robot })).Goto(states.Single(x => x.GetType() == guard.nextstate));
            }

            var FinalGuards = PagesAssembly.GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGuard<>)))
                .Select(x => new
                {
                    type = x,
                    currentstate = x.GetInterfaces().Single(y => y.GetGenericTypeDefinition() == typeof(IGuard<>)).GenericTypeArguments[0]
                })
                .Where(x => x.currentstate == state.GetType());
            foreach (var guard in FinalGuards)
            {
                var theguard = Activator.CreateInstance(guard.type);
                builder.In(state).On(RobotEvents.NormalTransition).If(() => (bool)guard.type.GetMethod("Condition").Invoke(theguard, new object[] { _robot })).Execute(() => _robot.Dispose());
            }
        }

        builder.WithInitialState(states.Single(x => x.GetType() == typeof(FirstPage)));
        _activeStateMachine = builder.Build().CreateActiveStateMachine();
        _watchDog = watchDog;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var generator = new StateMachineReportGenerator<BaseState, RobotEvents>();
        _activeStateMachine.Report(generator);

        string report = generator.Result;
        _logger.LogDebug(report);

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
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void _activeStateMachine_TransitionExceptionThrown(object sender, Appccelerate.StateMachine.Machine.Events.TransitionExceptionEventArgs<BaseState, RobotEvents> e)
    {
        _logger.LogCritical("EXCEPTION!!!!");
    }

    private void _activeStateMachine_TransitionCompleted(object sender, Appccelerate.StateMachine.Machine.Events.TransitionCompletedEventArgs<BaseState, RobotEvents> e)
    {
        _logger.LogWarning("TRANSIÇÃO REALIZADA!!!!");
        _watchDog.SetWatchDog();
    }
}