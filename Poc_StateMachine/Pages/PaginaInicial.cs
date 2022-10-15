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

internal class PaginaInicial : BaseState
{
    public PaginaInicial(string name, Robot robot) : base(name, robot)
    {
    }

    public override void Execute()
    {
        var navigate = new NavigationRequest()
        {
            Url = "https://novomeuespacocorretor.libertyseguros.com.br/"
        };
        _robot.Execute(navigate).Wait();
        var click = new ClickRequest()
        {
            By = By.XPath("//a[contains(text(),'Entrar no novo')]"),
            Timeout = new TimeSpan(0, 0, 5),
        };
        _robot.Execute(click).Wait();
    }
}