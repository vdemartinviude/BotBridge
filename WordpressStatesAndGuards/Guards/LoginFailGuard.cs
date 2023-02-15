using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;
using WordpressStatesAndGuards.States;

namespace WordpressStatesAndGuards.Guards;

public class LoginFailGuard : IGuard<WPLogin, StopRobotLoginError>
{
    public uint Priority => 5;

    public bool Condition(Robot robot)
    {
        var response = robot.ExecuteWithWait(new ElementExistRequest
        {
            By = By.XPath("//div[@id='login_error']")
        });
        if (response.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk && response.WebElement!.Displayed)
        {
            return true;
        }
        return false;
    }
}