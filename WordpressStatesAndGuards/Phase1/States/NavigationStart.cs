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

public class NavigationStart : BaseState
{
    public override TimeSpan StateTimeout => TimeSpan.FromSeconds(30);

    public NavigationStart(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("NavigationStart", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute()
    {
        _results.AddResultMessage("StartTime", $"Robot start working at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        await _robot.Execute(new NavigationRequest
        {
            Timeout = TimeSpan.FromSeconds(5),
            Url = _inputData.GetStringData("$.Url")
        });
    }
}