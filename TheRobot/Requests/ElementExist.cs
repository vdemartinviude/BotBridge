using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class ElementExist : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver> PreExecute { get; set; }
    public Action<IWebDriver> PostExecute { get; set; }
    public By By { get; set; }
    public TimeSpan Timeout { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        var wait = new WebDriverWait(driver, (TimeSpan)Timeout);

        IWebElement element = null;

        try
        {
            element = wait.Until(e =>
            {
                try
                {
                    var element = e.FindElement(By);
                    return element;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
        catch (Exception)
        {
            element = null;
        }

        if (element != null)
        {
            return new()
            {
                Status = RobotResponseStatus.ActionRealizedOk,
                WebElement = element
            };
        }
        else
        {
            return new()
            {
                Status = RobotResponseStatus.ElementNotFound,
                WebElement = null
            };
        }
    }
}