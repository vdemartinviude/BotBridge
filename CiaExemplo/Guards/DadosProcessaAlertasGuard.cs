using CiaExemplo.PagesStates;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace CiaExemplo.Guards;

public class DadosProcessaAlertasGuard : IGuard<DadosSeguro2, ProcessaAlertas>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        var element = robot.Execute(new ElementExist
        {
            By = By.XPath("//div[@role='alert']"),
            Timeout = TimeSpan.FromSeconds(5)
        }).Result;

        if (element.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk && element.WebElement.Displayed)
            return true;

        return false;
    }
}