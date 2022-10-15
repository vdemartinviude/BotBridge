using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;
using TheRobot.Helpers;
using OpenQA.Selenium.Support.UI;

namespace TheRobot.Requests;

public class GetElement : IRobotRequest
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
            throw new ArgumentNullException(nameof(By));
        }
        if (Timeout == null)
        {
            throw new ArgumentNullException(nameof(Timeout));
        }
        
        WebDriverWait wait = new WebDriverWait(driver, (TimeSpan) Timeout);
        try
        {
            webElement = wait.Until(e => e.FindElement(By));
            
        }
        catch(WebDriverTimeoutException)
        {
            return new()
            {
                WebElement = null,
                Status = RobotResponseStatus.ElementNotFound,
            };
          
        }
        
        return new()
        {
            WebElement = webElement,
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}
