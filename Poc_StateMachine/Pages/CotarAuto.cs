using Appccelerate.StateMachine;
using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace Poc_StateMachine.Pages;

internal class CotarAuto : BaseState
{
    public CotarAuto(string name, Robot robot) : base(name, robot)
    {
    }

    public override void Execute()
    {
        var clickCotar = new ClickRequest()
        {
            By = By.XPath("//a[contains(text(),'Cotar')]")
        };
        _robot.Execute(clickCotar).Wait();
    }
}