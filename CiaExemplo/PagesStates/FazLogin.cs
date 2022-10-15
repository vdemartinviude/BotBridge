using Json.Path;
using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace CiaExemplo.PagesStates;

public class FazLogin : BaseState
{
    public FazLogin(Robot robot, BaseOrcamento baseOrcamento) : base("FazLogin", robot, baseOrcamento)
    {
    }

    public override void Execute()
    {
        var pathCpf = JsonPath.Parse("$.DadosLogin.CpfLogin");
        var pathLogin = JsonPath.Parse("$.DadosLogin.Password");

        var setuser = new SetTextRequest()
        {
            By = By.XPath("//input[@name='username']"),
            Text = _orcamento.GetData(pathCpf)
        };
        _robot.Execute(setuser).Wait();
        var setpassword = new SetTextRequest()
        {
            By = By.Id("1-password"),
            Text = _orcamento.GetData(pathLogin)
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