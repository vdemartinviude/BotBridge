using Appccelerate.StateMachine;
using Appccelerate.StateMachine.AsyncMachine;
using JsonDocumentsManager;
using Microsoft.Extensions.Configuration;
using StatesAndEvents;
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
        public AsyncActiveStateMachine<BaseState, MachineEvents>? Machine { get; private set; }

        public TheMachine(MachineInfrastructure machineInfrastructure, IConfiguration configuration)
        {
            _machineSpecification = machineInfrastructure.MachineSpecification;
            _robot = machineInfrastructure.Robot;
            _inputDocument = machineInfrastructure.InputJsonDocument;
            _resultDocument = machineInfrastructure.ResultJsonDocument;
            _configuration = configuration;
        }

        public void Build()
        {
            StateMachineDefinitionBuilder<BaseState, MachineEvents> builder = new();
            BaseState? theFirstState = null;

            var states = _machineSpecification.States.Select(st =>
                (BaseState)Activator.CreateInstance(st, new object[] { _robot, _inputDocument, _resultDocument })!).ToList();

            foreach (var state in states)
            {
                builder.In(state).ExecuteOnEntry(() => state.MainExecute(Machine!)).On(MachineEvents.FinalizeMachine).Execute(() => _finalizaMaquina());
                foreach (var guard in _machineSpecification.IntermediaryGuards.Where(guard => guard.CurrentState.Name == state.GetType().Name))
                {
                    var theguard = Activator.CreateInstance(guard.Guard);
                    var nextstate = states.Where(st => st.GetType().Name == guard.NextState.Name).Single();

                    builder.In(state).On(MachineEvents.NormalTransition)
                        .If(() => (bool)guard.Guard.GetMethod("Condition")!.Invoke(theguard, new object[] { _robot })!)
                        .Goto(nextstate);
                }

                foreach (var guard in _machineSpecification.FinalGuards.Where(guard => guard.CurrentState.Name == state.GetType().Name))
                {
                    var theguard = Activator.CreateInstance(guard.Guard);
                    builder.In(state).On(MachineEvents.NormalTransition)
                        .If(() => (bool)guard.Guard.GetMethod("Condition")!.Invoke(theguard, new object[] { _robot })!)
                        .Execute(() => _normalFinish());
                }
                if (!_machineSpecification.IntermediaryGuards.Any(g => g.NextState.Name == state.GetType().Name))
                    theFirstState = state;
            }

            builder.WithInitialState(theFirstState!);
            Machine = builder.Build().CreateActiveStateMachine();
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