using Liberty.PagesStates;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.Guards;

public class SelecaoCanalGuard : IGuard<SelecaoCanal, MeuCotador>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        var element = robot.Execute(new WaitElementBeClickableRequest
        {
            By = By.XPath("//input[@id='Acessar']"),
            Timeout = TimeSpan.FromSeconds(7)
        }).Result;

        if (element.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk)
        {
            return true;
        }
        return false;
    }
}