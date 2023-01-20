using Liberty.PagesStates;
using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.Guards;

public class AcessaCotacaoGuard : IGuard<AcessaCotacaoAuto, SelecaoCanal>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        var element = robot.Execute(new WaitAndMoveElementSelectable
        {
            Timeout = TimeSpan.FromSeconds(1),
            By = By.Id("ddlBranch")
        }).Result;
        if (element.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk)
        {
            return true;
        }
        return false;
    }
}