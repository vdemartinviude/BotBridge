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

public class FirstPageGuard : IGuard<FirstPage, FazLogin>
{
    public bool Condition(Robot robot)
    {
        IWebElement? wait = new WebDriverWait(robot._driver, TimeSpan.FromSeconds(10))
            .Until(ExpectedConditions.ElementExists(By.XPath("//input[@name='username']")));
        IJavaScriptExecutor executor = (IJavaScriptExecutor)robot._driver;
        executor.ExecuteScript("arguments[0].scrollIntoView();", wait);
        var wait2 = new WebDriverWait(robot._driver, TimeSpan.FromSeconds(15))
            .Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@name='username']")));

        return true;
    }
}