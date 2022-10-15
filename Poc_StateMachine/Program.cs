using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Appccelerate.StateMachine.AsyncMachine.States;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using StatesAndEvents;
using System.Collections.Generic;
using OpenQA.Selenium;
using System;
using System.Linq;
using Poc_StateMachine.Pages;
using TheRobot;
using StatesAndEvents.Helpers;
using System.Reflection;
using CiaExemplo.Guards;
using CiaExemplo.PagesStates;
using System.ComponentModel.DataAnnotations;
using Appccelerate.StateMachine.AsyncSyntax;

internal class Program
{
    private static PassiveStateMachine<BaseState, int> _passiveStateMachine;

    private static void Main(string[] args)
    {
        StateMachineDefinitionBuilder<BaseState, int> builder = new();
        var PagesAssembly = Assembly.Load("CiaExemplo");

        Robot robot = new();

        // Populate List of states
        List<BaseState> states = new();
        states.AddRange(PagesAssembly.ExportedTypes.Where(x => x.BaseType == typeof(BaseState))
            .Select(x => (BaseState)Activator.CreateInstance(x, new object[] { robot })));

        foreach (var state in states)
        {
            builder.In(state).ExecuteOnEntry(() => state.MainExecute(_passiveStateMachine));
        }

        //foreach (var state in states)
        //{
        //    var bu = builder.In(state).ExecuteOnEntry(() => state.MainExecute(_passiveStateMachine));

        //    // TODO: Here we are looking just for intermediate states. We need to look to the final state too!

        //    var Guards = PagesAssembly.GetExportedTypes()
        //        .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGuard<,>)))
        //        .Select(x => new
        //        {
        //            type = x,
        //            inter = x.GetInterfaces().Single(y => y.GetGenericTypeDefinition() == typeof(IGuard<,>))
        //        })
        //        .Where(x => x.inter.GenericTypeArguments[0] == state.GetType());

        //    if (Guards.Count() > 0)
        //    {
        //        var bu2 = bu.On(1);

        //        Appccelerate.StateMachine.Syntax.IGotoInIfSyntax<BaseState, int> bu3;
        //        bu3 = null;
        //        foreach (var guard in Guards)
        //        {
        //            var theguard = Activator.CreateInstance(guard.type);
        //            bu3 = (bu3 == null) ? bu2.If(() => (bool)guard.type.GetMethod("Condition").Invoke(theguard, new object[] { robot }))
        //                .Goto(states.Single(x => x.GetType() == guard.inter.GenericTypeArguments[1])) :
        //                bu3.If(() => (bool)guard.type.GetMethod("Condition").Invoke(theguard, new object[] { robot }))
        //                .Goto(states.Single(x => x.GetType() == guard.inter.GenericTypeArguments[1]));
        //        }
        //    }
        //}

        //foreach (var y in x.ExportedTypes.Where(x => x.BaseType == typeof(BaseState)))
        //{
        //    var z = (BaseState)Activator.CreateInstance(y, new object[] { robot });
        //    var Guards = x.ExportedTypes.Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGuard<,>)))
        //        .Select(x => new
        //        {
        //            type = x,
        //            inter = x.GetInterfaces().Single(y => y.GetGenericTypeDefinition() == typeof(IGuard<,>))
        //        })
        //        .Where(x => x.inter.GenericTypeArguments[0] == y);

        //    //var Inters2 = Inters.SelectMany(x => x.GenericTypeArguments);

        //    states.Add(z);
        //    var bu = builder
        //        .In(z)
        //        .ExecuteOnEntry(() => z.MainExecute(_passiveStateMachine))
        //        .On(1);

        //    foreach (var guard in Guards)
        //    {
        //        var theguard = (IGuard<BaseState,BaseState>) Activator.CreateInstance(guard.type);
        //        bu = bu.If(() => theguard.Condition(robot)).Goto()

        //    }
        //        .If(() => new FirstPageGuard().Condition(robot))
        //        .Execute(() => stopMachine());
        //}
        //builder.WithInitialState(states.Where(a => a.Name == "FirstPage").Single());
        //_passiveStateMachine = builder.Build().CreatePassiveStateMachine();
        //_passiveStateMachine.Start();

        #region oldcode

        //List<BaseState> _states = new List<BaseState>()
        //{
        //    new PaginaInicial("PrimeiraPagina",robot,_passiveStateMachine),
        //    new Login("SegundaPagina",robot,_passiveStateMachine),
        //    new CotarAuto("CotarAuto",robot,_passiveStateMachine)
        //};

        //var builder = new StateMachineDefinitionBuilder<BaseState,int>();

        //builder
        //    .In(_states.Single(a => a.Name == "PrimeiraPagina"))
        //    .ExecuteOnEntry(_states.Single(a => a.Name == "PrimeiraPagina").MainExecute)
        //    .On(1)
        //    .If(() => Guards.ExistMoveClickable(By.XPath("//input[@name='username']"), robot))
        //    .Goto(_states.Single(a => a.Name == "SegundaPagina"))
        //    .If(() => Guards.ExistMoveClickable(By.XPath("//alert"), robot))
        //    .Execute(StopMachine);

        //builder
        //    .In(_states.Single(a => a.Name == "SegundaPagina"))
        //    .ExecuteOnEntry((_states.Single(a => a.Name == "SegundaPagina").MainExecute))
        //    .On(1)
        //    .If(() => Guards.ExistMoveClickable(By.XPath("//a[contains(text(),'Cotar')]"),robot))
        //    .Goto(_states.Single(a => a.Name == "CotarAuto"));

        //builder
        //    .In(_states.Single(a => a.Name == "CotarAuto"))
        //    .ExecuteOnEntry((_states.Single(a => a.Name == "CotarAuto").MainExecute))
        //    .On(1)
        //    .If(() => Guards.ElementSelectable(By.Id("ddlBranch"),robot))
        //    .Execute(() => StopMachine());

        builder.WithInitialState(states.Single(a => a.Name == "FirstPage"));

        _passiveStateMachine = builder
            .Build()
            .CreatePassiveStateMachine();

        _passiveStateMachine.Start();

        //while (_passiveStateMachine.IsRunning)
        //{
        //}

        #endregion oldcode

        while (_passiveStateMachine.IsRunning) ;
        //Console.WriteLine("Tchau");
        robot.Dispose();
    }

    private static void stopMachine()
    {
        _passiveStateMachine.Stop();
    }
}