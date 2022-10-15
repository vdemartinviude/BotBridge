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

namespace CiaExemplo.Guards;

public class LibertyAutoPerfilGuard : IGuard<LibertyAutoPerfil, DadosPrincipais>
{
    public bool Condition(Robot robot)
    {
        try
        {
            var wait = new WebDriverWait(robot._driver, TimeSpan.FromSeconds(30))
                .Until(a => a.FindElement(By.XPath("//label[contains(text(),'É uma renovação?')]")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}