using Liberty.PagesStates;
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

namespace Liberty.Guards;

public class LibertyAutoPerfilGuard : IGuard<LibertyAutoPerfil, DadosPrincipais>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        var element = robot.Execute(new ElementExistRequest
        {
            Timeout = TimeSpan.FromSeconds(30),
            By = By.XPath("//label[contains(text(),'É uma renovação?')]")
        }).Result;

        if (element.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk && element.WebElement!.Displayed)
            return true;

        return false;
    }
}