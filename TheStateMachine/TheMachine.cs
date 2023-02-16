using Appccelerate.StateMachine;
using Appccelerate.StateMachine.AsyncMachine;
using JsonDocumentsManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StatesAndEvents;
using System.Reflection;
using System.Threading;
using TheRobot;
using TheStateMachine.Model;

namespace TheStateMachine
{
    public class TheMachine
    {
        private readonly MachineSpecification _machineSpecification;
        private readonly Robot _robot;
        private readonly InputJsonDocument _inputDocument;
        private readonly ResultJsonDocument _resultDocument;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TheMachine> _logger;
        public AsyncActiveStateMachine<BaseState, MachineEvents>? Machine { get; private set; }

        public TheMachine(MachineInfrastructure machineInfrastructure, IConfiguration configuration, ILogger<TheMachine> logger)
        {
            _machineSpecification = machineInfrastructure.MachineSpecification;
            _robot = machineInfrastructure.Robot;
            _inputDocument = machineInfrastructure.InputJsonDocument;
            _resultDocument = machineInfrastructure.ResultJsonDocument;
            _configuration = configuration;
            _logger = logger;
        }

        public void Build()
        {
            StateMachineDefinitionBuilder<BaseState, MachineEvents> builder = new();
            BaseState theFirstState = null;

            var states = _machineSpecification.States
                .Select(st => (BaseState)Activator.CreateInstance(st, new object[] { _robot, _inputDocument, _resultDocument })).ToList();
            foreach (var state in states)
            {
                builder.In(state).ExecuteOnEntry(() => MachineExecuteState(state));
                builder.In(state).On(MachineEvents.FinalizeMachine).Execute(() => _finalizaMaquina());
                var interGuardsForState = _machineSpecification.IntermediaryGuards.Where(x => x.CurrentState.Name == state.GetType().Name).
                    Select(x => new
                    {
                        CurrentState = x.CurrentState,
                        NextState = x.NextState,
                        TheGuard = Activator.CreateInstance(x.Guard, new object[] { }),
                        Guard = x.Guard
                    })
                    .OrderBy(x => (uint)x.Guard.GetProperty("Priority").GetValue(x.TheGuard));
                foreach (var guard in interGuardsForState)
                {
                    var nextstate = states.Where(x => x.GetType().Name == guard.NextState.Name).Single();
                    builder
                        .In(state)
                        .On(MachineEvents.NormalTransition)
                        .If(() => (bool)guard.Guard.GetMethod("Condition").Invoke(guard.TheGuard, new object[] { _robot }))
                        .Goto(nextstate);
                }
                var finalGuardsForState = _machineSpecification.FinalGuards.Where(x => x.CurrentState.Name == state.GetType().Name)
                    .Select(x => new
                    {
                        CurrentState = x.CurrentState,
                        TheGuard = Activator.CreateInstance(x.Guard, new object[] { }),
                        Guard = x.Guard
                    });
                foreach (var guard in finalGuardsForState)
                {
                    builder
                        .In(state)
                        .On(MachineEvents.NormalTransition)
                        .If(() => (bool)guard.Guard.GetMethod("Condition").Invoke(guard.TheGuard, new object[] { _robot }))
                        .Execute(() => _normalFinish());
                }
                builder.In(state).On(MachineEvents.NormalTransition).Execute(() => _finalizaMaquina());
                if (!_machineSpecification.IntermediaryGuards.Any(g => g.NextState.Name == state.GetType().Name))
                    theFirstState = state;
            }
            builder.WithInitialState(theFirstState!);
            Machine = builder.Build().CreateActiveStateMachine();
        }

        private void MachineExecuteState(BaseState state)
        {
            AutoResetEvent autoResetEvent = new(false);
            ThreadPool.RegisterWaitForSingleObject(autoResetEvent, new WaitOrTimerCallback(MyCallBackFunction), state, (int)state.StateTimeout.TotalMilliseconds, true);
            state.MainExecute(Machine);
            autoResetEvent.Set();
        }

        private void MyCallBackFunction(object? state, bool timedOut)
        {
            if (timedOut)
            {
                _logger.LogCritical("State {name} timeout", state.GetType().Name);
                _robot.Dispose();
                Machine.Stop();
            }
        }

        public void ExecuteMachine()
        {
            Machine!.Start();
        }

        private void _normalFinish()
        {
            _robot.Dispose();
            Machine!.Stop();
        }

        private void _finalizaMaquina()
        {
            _robot.Dispose();
            Machine!.Stop();
        }
    }
}