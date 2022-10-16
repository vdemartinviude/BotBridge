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

namespace CiaExemplo.Guards;

public class DadosProcessaAlertasGuard : IGuard<DadosSeguro2, ProcessaAlertas>
{
    public bool Condition(Robot robot)
    {
        try
        {
            var wait = new WebDriverWait(robot._driver, TimeSpan.FromSeconds(5)).Until(x => x.FindElement(By.XPath("//div[@role='alert']")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}