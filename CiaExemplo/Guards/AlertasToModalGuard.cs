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

public class AlertasToModalGuard : IGuard<ProcessaAlertas, ProcessaModal>
{
    public bool Condition(Robot robot)
    {
        try
        {
            var modal = new WebDriverWait(robot._driver, TimeSpan.FromSeconds(7)).Until(x => x.FindElement(By.XPath("//div[@class='modal-master-header modal-master-header-warning']")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}