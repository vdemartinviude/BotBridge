using JsonDocumentsManager;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace WordpressStatesAndGuards.States;

public class NavigateToLogin : BaseState
{
    public NavigateToLogin(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("NavigateToLogin", robot, inputdata, resultJson)
    {
    }

    public override void Execute()
    {
        _robot.ExecuteWithWait(new NavigationRequest
        {
            Url = "http://localhost:8000/wp-admin"
        });
    }
}