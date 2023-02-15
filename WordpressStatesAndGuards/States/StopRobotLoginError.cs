using JsonDocumentsManager;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace WordpressStatesAndGuards.States;

public class StopRobotLoginError : BaseState
{
    public StopRobotLoginError(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("StopRobotLoginError", robot, inputdata, resultJson)
    {
    }

    public override void Execute()
    {
        _results.AddResultMessage("Error", "The robot can not execute because a login error ocorred");
        _results.AddResultMessage("EndTime", $"The robot has ended working at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        _results.SaveDocument("result.json").Wait();
    }
}