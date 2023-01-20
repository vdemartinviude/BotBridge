using Appccelerate.StateMachine;
using Appccelerate.StateMachine.AsyncMachine;
using JsonDocumentsManager;
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
        public AsyncActiveStateMachine<BaseState, MachineEvents>? Machine { get; private set; }

        public TheMachine(MachineSpecification machineSpecification, Robot robot, InputJsonDocument input, ResultJsonDocument resultJsonDocument)
        {
            _machineSpecification = machineSpecification;
            _robot = robot;
            _inputDocument = input;
            _resultDocument = resultJsonDocument;
        }

        public void Build()
        {
            StateMachineDefinitionBuilder<BaseState, MachineEvents> builder = new();
            BaseState? theFirstState = null;
            foreach (var state in _machineSpecification.States)
            {
                var createdState = (BaseState)Activator.CreateInstance(state, new object[] { _robot, _inputDocument, _resultDocument })!;
                builder.In(createdState).ExecuteOnEntry(() => createdState.MainExecute(Machine!)).On(MachineEvents.FinalizeMachine).Execute(() => _finalizaMaquina())
                    .On(MachineEvents.NormalTransition).Execute(() => _normalFinish());

                if (!_machineSpecification.IntermediaryGuards.Any(g => g.NextState.Name == state.Name))
                    theFirstState = createdState;
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