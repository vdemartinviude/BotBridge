using Appccelerate.StateMachine.AsyncMachine;
using Appccelerate.StateMachine.Machine;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TheStateMachine.Model;

namespace TheStateMachine.Helpers;

public static class TheStateMachineHelpers
{
    public static MachineSpecification GetMachineSpecification(Assembly assembly)
    {
        var specification = new MachineSpecification();
        specification.States = new();

        var statesInAssembly = assembly.ExportedTypes.Where(type => type.BaseType == typeof(BaseState))
            .Select(x => (BaseState)Activator.CreateInstance(x, new object[] { null, null, null }));
        return specification;
    }
}
