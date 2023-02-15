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
    public WPInitialConfiguration(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("WPInitialConfiguration", robot, inputdata, resultJson)
    {
    }

    public override void Execute()
    {
        _robot.ExecuteWithWait(new SelectTextRequest
        {
            By = By.Id("language"),
            Timeout = TimeSpan.FromSeconds(5),
            Text = String.IsNullOrEmpty(_inputData.GetStringData("$.Language")) ? "Português do Brasil" : _inputData.GetStringData("$.Language")
        });

        _robot.ExecuteWithWait(new ClickRequest
        {
            By = By.Id("language-continue"),
        });

        _robot.ExecuteWithWait(new SetTextRequest
        {
            By = By.Id("weblog_title"),
            Text = _inputData.GetStringData("$.Title")
        });

        _robot.ExecuteWithWait(new SetTextRequest
        {
            By = By.Id("user_login"),
            Text = _inputData.GetStringData("$.UserLogin")
        });

        _robot.ExecuteWithWait(new SetTextRequest
        {
            By = By.Id("pass1"),
            Text = _inputData.GetStringData("$.Password"),
            ClearBefore = true,
        });

        _robot.ExecuteWithWait(new SetTextRequest
        {
            By = By.Id("admin_email"),
            Text = _inputData.GetStringData("$.Email")
        });

        _robot.ExecuteWithWait(new ClickRequest
        {
            By = By.Id("submit")
        });

        _robot.ExecuteWithWait(new ClickRequest
        {
            By = By.XPath("//a[1]")
        });
    }
}