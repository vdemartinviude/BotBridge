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

namespace CiaExemplo.Guards;

public class ErroLoginPageGuard : IGuard<FazLogin, ErroLogin>
{
    public bool Condition(Robot robot)
    {
        try
        {
            IWebElement wait = new WebDriverWait(robot._driver, TimeSpan.FromSeconds(4))
                .Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[text()='Usuário ou senha inválidos.']")));
            return true;
        }
        catch (OpenQA.Selenium.WebDriverTimeoutException)
        {
            return false;
        }
    }
}

public class ErroLoginPageGuard2 : IGuard<FazLogin, ErroLogin>
{
    public bool Condition(Robot robot)
    {
        try
        {
            IWebElement wait = new WebDriverWait(robot._driver, TimeSpan.FromSeconds(4))
                .Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(text(),'bloqueada')]")));
            return true;
        }
        catch (OpenQA.Selenium.WebDriverTimeoutException)
        {
            return false;
        }
    }
}

public class LoginComSucessoGuard : IGuard<FazLogin, AcessaCotacaoAuto>
{
    public bool Condition(Robot robot)
    {
        return WaitTransitions.ExistMoveClickable(By.XPath("//a[contains(text(),'Cotar')]"), robot);
    }
}