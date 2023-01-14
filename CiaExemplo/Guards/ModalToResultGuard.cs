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
using SeleniumExtras.WaitHelpers;
using TheRobot.Requests;

namespace CiaExemplo.Guards;

public class ModalToResultGuard : IGuard<ProcessaModal, ProcessaResultado>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        var element = robot.Execute(new WaitElementBeClickableRequest
        {
            Timeout = TimeSpan.FromSeconds(5),
            By = By.Id("btnShowModalCoberturas")
        }).Result;

        if (element.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk)
        {
            return true;
        }
        return false;
    }
}