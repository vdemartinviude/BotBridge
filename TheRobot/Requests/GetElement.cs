using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;
using TheRobot.Helpers;

namespace TheRobot.Requests;

public class GetElement : IRoboRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public TimeSpan? Timeout { get; set; }
    public By? By { get; set; }
    public RobotResponse Exec(IWebDriver driver)
    {
        IWebElement webElement = null;
        if (By == null)
        {
            throw new ArgumentNullException("You must specify the element using By");
        }
        if (Timeout == null)
        {
            throw new ArgumentNullException("You must specify a timeout");
        }
        RobotResponse resp;

        try
        {
            RobotHelpers.DoOrTimeout(() => webElement = driver.FindElement(By), (TimeSpan)Timeout);
            resp = new RobotResponse
            {
                WebElement = webElement,
                Status = RobotResponseStatus.ActionRealizedOk
            };
            return resp;
        }
        catch(Exception ex)
        {
            resp = new RobotResponse
            {
                WebElement = null,
                Status = RobotResponseStatus.ElementNotFound,
            };
            return resp;
        }

    }
}
