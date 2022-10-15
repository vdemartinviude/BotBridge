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

public class MeuCotadorGuard : IGuard<MeuCotador, LibertyAutoPerfil>
{
    public bool Condition(Robot robot)
    {
        try
        {
            var wait = new WebDriverWait(robot._driver, TimeSpan.FromSeconds(5))
                .Until(a => a.FindElement(By.XPath("//h1[contains(text(),'Meu Cotador')]")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}