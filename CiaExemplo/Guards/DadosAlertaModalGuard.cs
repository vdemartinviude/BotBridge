using Liberty.PagesStates;
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

namespace Liberty.Guards;

public class DadosAlertaModalGuard : IGuard<DadosSeguro2, ProcessaModal>
{
    public uint Priority => 5;

    public bool Condition(Robot robot)
    {
        var modal = robot.Execute(new ElementExistRequest
        {
            By = By.Id("modalCotacao"),
            Timeout = TimeSpan.FromSeconds(7)
        }).Result;
        if (modal.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk && modal.WebElement.Displayed)
            return true;
        return false;
    }
}