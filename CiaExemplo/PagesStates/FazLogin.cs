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

    public override async Task Execute(CancellationToken token)
    {
        var pathCpf = JsonPath.Parse("$.DadosLogin.CpfLogin");
        var pathLogin = JsonPath.Parse("$.DadosLogin.Password");

        await _robot.Execute(new SetTextRequest()
        {
            By = By.XPath("//input[@name='username']"),
            Text = _inputData.GetStringData(pathCpf)
        });
        await _robot.Execute(new SetTextRequest()
        {
            By = By.Id("1-password"),
            Text = _inputData.GetStringData(pathLogin)
        });

        await _robot.Execute(new ClickRequest()
        {
            By = By.Id("1-submit"),
            DelayAfter = new TimeSpan(0, 0, 15)
        });

        await _robot.Execute(new ClickRequest()
        {
            By = By.Id("edit-agree"),
            Timeout = TimeSpan.FromSeconds(1)
        });

        await _robot.Execute(new ClickRequest()
        {
            By = By.Id("edit-submit"),
            Timeout = TimeSpan.FromSeconds(2),
            DelayAfter = TimeSpan.FromSeconds(7)
        });
    }
}