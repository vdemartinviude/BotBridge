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

public class WPLogin : BaseState
{
    public override TimeSpan StateTimeout => TimeSpan.FromMinutes(10);

    public WPLogin(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("WPLogin", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        await _robot.Execute(new SetTextRequest
        {
            By = By.Id("user_login"),
            Text = _inputData.GetStringData("$.UserLogin")
        });
        await _robot.Execute(new SetTextRequest
        {
            By = By.Id("user_pass"),
            Text = _inputData.GetStringData("$.Password")
        });

        await _robot.Execute(new ClickRequest
        {
            By = By.Id("wp-submit")
        });
    }
}