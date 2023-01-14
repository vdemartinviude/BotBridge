using CiaExemplo.PagesStates;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace CiaExemplo.Guards;

public class AlertasToModalGuard : IGuard<ProcessaAlertas, ProcessaModal>
{
    public uint Priority => 5;

    public bool Condition(Robot robot)
    {
        var modal = robot.Execute(new ElementExist
        {
            By = By.XPath("//div[@class='modal-master-header modal-master-header-warning']"),
            Timeout = TimeSpan.FromSeconds(7)
        }).Result;

        if (modal.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk && modal.WebElement.Displayed)
            return true;
        return false;
    }
}