using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace StatesAndEvents.Helpers;

public static class WaitTransitions
{
    public static bool ExistTab(Robot robot)
    {
        return false;
    }

    public static bool ElementSelectable(By by, Robot robot)
    {
        try
        {
            IWebElement? wait = new WebDriverWait(robot._driver, new TimeSpan(0, 0, 0, 0, 100))
                .Until(ExpectedConditions.ElementExists(by));
            if (wait == null) return false;
            IJavaScriptExecutor executor = (IJavaScriptExecutor)robot._driver;
            executor.ExecuteScript("arguments[0].scrollIntoView();", wait);

            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
        catch (OpenQA.Selenium.WebDriverTimeoutException)
        {
            return false;
        }
    }

    public static bool ExistMoveClickable(By by, Robot robot)
    {
        try
        {
            IWebElement? wait = new WebDriverWait(robot._driver, new TimeSpan(0, 0, 0, 0, 500))
                .Until(ExpectedConditions.ElementExists(by));
            if (wait == null) return false;
            IJavaScriptExecutor executor = (IJavaScriptExecutor)robot._driver;
            executor.ExecuteScript("arguments[0].scrollIntoView();", wait);
            var wait2 = new WebDriverWait(robot._driver, new TimeSpan(0, 0, 0, 0, 500))
                .Until(ExpectedConditions.ElementToBeClickable(by));

            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
        catch (OpenQA.Selenium.WebDriverTimeoutException)
        {
            return false;
        }
    }
}