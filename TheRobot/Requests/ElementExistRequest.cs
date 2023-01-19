using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class ElementExistRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver> PreExecute { get; set; }
    public Action<IWebDriver> PostExecute { get; set; }
    public By By { get; set; }
    public TimeSpan? Timeout { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        var wait = new WebDriverWait(driver, Timeout.Value);

        IWebElement element = null;

        element = wait.Until(d => d.FindElement(By));
        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk,
            WebElement = element
        };
    }
}