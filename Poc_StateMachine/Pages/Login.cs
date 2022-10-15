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

public class Login : BaseState
{
    public Login(string name, Robot robot) : base(name, robot)
    {
    }

    public override void Execute()
    {
        Console.WriteLine("Estou na pagina 1");

        var setuser = new SetTextRequest()
        {
            By = By.XPath("//input[@name='username']"),
            Text = "31141264803"
        };
        _robot.Execute(setuser).Wait();
        var setpassword = new SetTextRequest()
        {
            By = By.Id("1-password"),
            Text = "Minha casa minha vida"
        };
        _robot.Execute(setpassword).Wait();

        var clicklogin = new ClickRequest()
        {
            By = By.Id("1-submit"),
            DelayAfter = new TimeSpan(0, 0, 15)
        };
        _robot.Execute(clicklogin).Wait();
    }
}