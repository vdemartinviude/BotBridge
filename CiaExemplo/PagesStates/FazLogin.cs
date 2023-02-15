using Json.Path;
using JsonDocumentsManager;
using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.PagesStates;

public class FazLogin : BaseState
{
    public FazLogin(Robot robot, InputJsonDocument baseOrcamento, ResultJsonDocument resultJson) : base("FazLogin", robot, baseOrcamento, resultJson)
    {
    }

    public override void Execute()
    {
        var pathCpf = JsonPath.Parse("$.DadosLogin.CpfLogin");
        var pathLogin = JsonPath.Parse("$.DadosLogin.Password");

        var setuser = new SetTextRequest()
        {
            By = By.XPath("//input[@name='username']"),
            Text = _inputData.GetStringData(pathCpf)
        };
        _robot.Execute(setuser).Wait();
        var setpassword = new SetTextRequest()
        {
            By = By.Id("1-password"),
            Text = _inputData.GetStringData(pathLogin)
        };
        _robot.Execute(setpassword).Wait();

        var clicklogin = new ClickRequest()
        {
            By = By.Id("1-submit"),
            DelayAfter = new TimeSpan(0, 0, 15)
        };
        _robot.Execute(clicklogin).Wait();

        _robot.Execute(new ClickRequest()
        {
            By = By.Id("edit-agree"),
            Timeout = TimeSpan.FromSeconds(1)
        }).Wait();

        _robot.Execute(new ClickRequest()
        {
            By = By.Id("edit-submit"),
            Timeout = TimeSpan.FromSeconds(2),
            DelayAfter = TimeSpan.FromSeconds(7)
        }).Wait();

        //_robot.Execute(new ClickRequest()
        //{
        //    By = By.XPath("//h4[@class='legal-terms-heading']/../../div/button"),
        //    DelayAfter = TimeSpan.FromSeconds(7)
        //}).Wait();

        //_robot.Execute(setuser).Wait();

        //_robot.Execute(setpassword).Wait();

        //_robot.Execute(clicklogin).Wait();
    }
}