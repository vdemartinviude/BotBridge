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
    public override TimeSpan StateTimeout => TimeSpan.FromSeconds(60);

    public NavigateToLogin(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("NavigateToLogin", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        await _robot.Execute(new NavigationRequest
        {
            Url = "http://localhost:8000/wp-admin"
        });
    }
}