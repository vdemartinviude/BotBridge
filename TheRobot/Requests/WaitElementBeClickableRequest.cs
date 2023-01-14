using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class WaitElementBeClickableRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver> PreExecute { get; set; }
    public Action<IWebDriver> PostExecute { get; set; }
    public TimeSpan Timeout { get; set; }
    public By By { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        try
        {
            var wait = new WebDriverWait(driver, Timeout)
                .Until(ExpectedConditions.ElementToBeClickable(By));

            return new()
            {
                WebElement = wait,
                Status = RobotResponseStatus.ActionRealizedOk
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                Status = RobotResponseStatus.ElementNotFound
            };
        }
    }
}