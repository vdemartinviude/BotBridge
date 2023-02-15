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
    public NavigationStart(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("NavigationStart", robot, inputdata, resultJson)
    {
    }

    public override void Execute()
    {
        _results.AddResultMessage("StartTime", $"Robot starting working at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        _robot.ExecuteWithWait(new NavigationRequest
        {
            Timeout = TimeSpan.FromSeconds(5),
            Url = _inputData.GetStringData("$.Url")
        });
    }
}