using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;
using WordpressStatesAndGuards.States;

namespace WordpressStatesAndGuards.Guards;

public class WpNotConfigGuard : IGuard<NavigationStart, WPInitialConfiguration>
{
    public uint Priority => 5;

    public bool Condition(Robot robot)
    {
        var languageSelectExist = robot.Execute(new ElementExistRequest
        {
            By = By.Id("language"),
            Timeout = TimeSpan.FromSeconds(5)
        }).Result;
        if (languageSelectExist.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk)
        {
            return true;
        }
        return false;
    }
}