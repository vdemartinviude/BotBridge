using CiaExemplo.PagesStates;
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

namespace CiaExemplo.Guards;

public class SelecaoCanalGuard : IGuard<SelecaoCanal, MeuCotador>
{
    public bool Condition(Robot robot)
    {
        try
        {
            var wait = new WebDriverWait(robot._driver, TimeSpan.FromSeconds(7))
                .Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@id='Acessar']")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}