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
    private PassiveStateMachine<BaseState, RobotEvents> _passiveStateMachine;

    public Worker(ILogger<Worker> logger, Robot robot, BaseOrcamento baseOrcamento)
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
            builder.In(state).ExecuteOnEntry(() => state.MainExecute(_passiveStateMachine));
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
        _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var generator = new StateMachineReportGenerator<BaseState, RobotEvents>();
        _passiveStateMachine.Report(generator);

        string report = generator.Result;
        _logger.LogDebug(report);
        try
        {
            _passiveStateMachine.Start();
        }
        catch (Appccelerate.StateMachine.Machine.StateMachineException ex)
        {
            _logger.LogCritical(ex.InnerException.Message);
        }
        //while (!stoppingToken.IsCancellationRequested)
        //{
        //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        //    await Task.Delay(1000, stoppingToken);
        //}
    }
}