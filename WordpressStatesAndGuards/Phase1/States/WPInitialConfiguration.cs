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

namespace WordpressStatesAndGuards.States;

public class WPInitialConfiguration : BaseState
{
    public override TimeSpan StateTimeout => TimeSpan.FromMinutes(10);

    public WPInitialConfiguration(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("WPInitialConfiguration", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        await _robot.Execute(new SelectTextRequest
        {
            By = By.Id("language"),
            Timeout = TimeSpan.FromSeconds(5),
            Text = String.IsNullOrEmpty(_inputData.GetStringData("$.Language")) ? "Português do Brasil" : _inputData.GetStringData("$.Language")
        });

        await _robot.Execute(new ClickRequest
        {
            By = By.Id("language-continue"),
        });

        await _robot.Execute(new SetTextRequest
        {
            By = By.Id("weblog_title"),
            Text = _inputData.GetStringData("$.Title")
        });

        await _robot.Execute(new SetTextRequest
        {
            By = By.Id("user_login"),
            Text = _inputData.GetStringData("$.UserLogin")
        });

        await _robot.Execute(new SetTextRequest
        {
            By = By.Id("pass1"),
            Text = _inputData.GetStringData("$.Password"),
            ClearBefore = true,
        });

        await _robot.Execute(new SetTextRequest
        {
            By = By.Id("admin_email"),
            Text = _inputData.GetStringData("$.Email")
        });

        await _robot.Execute(new ClickRequest
        {
            By = By.Id("submit")
        });

        await _robot.Execute(new ClickRequest
        {
            By = By.XPath("//a[1]")
        });
    }
}