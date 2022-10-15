// See https://aka.ms/new-console-template for more information

using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using CiaExemplo.PagesStates;
using StatesAndEvents;
using TheRobot;

PassiveStateMachine<BaseState, int> passiveStateMachine = null;

StateMachineDefinitionBuilder<BaseState, int> builder = new();
Robot robot = new();
List<BaseState> states = new()
{
    new FirstPage(robot),
};

builder
    .In(states[0])
    .ExecuteOnEntry(() => states[0].MainExecute(passiveStateMachine));

Console.WriteLine("Hello, World!");