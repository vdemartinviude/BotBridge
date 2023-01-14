using CiaExemplo.PagesStates;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using StatesAndEvents;
using StatesAndEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Helpers;
using TheRobot.Requests;

namespace CiaExemplo.Guards;

public class ErroLoginPageGuard : IGuard<FazLogin, ErroLogin>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        var request = new ElementExist()
        {
            By = By.XPath("//span[text()='Usuário ou senha inválidos.']"),
            Timeout = TimeSpan.FromSeconds(4)
        };
        if (robot.Execute(request).Result.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class ErroLoginPageGuard2 : IGuard<FazLogin, ErroLogin>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        var request = new ElementExist()
        {
            By = By.XPath("//span[contains(text(),'bloqueada')]"),
            Timeout = TimeSpan.FromSeconds(4)
        };
        var robotResult = robot.Execute(request).Result;

        if (robotResult.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk && robotResult.WebElement.Displayed == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class LoginComSucessoGuard : IGuard<FazLogin, AcessaCotacaoAuto>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        var element = robot.Execute(new WaitAndMoveToElementClickableRequest
        {
            Timeout = TimeSpan.FromSeconds(4),
            By = By.XPath("//a[contains(text(),'Cotar')]")
        }).Result;

        if (element.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk)
        {
            return true;
        }
        return false;
    }
}