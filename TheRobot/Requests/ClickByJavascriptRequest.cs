using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class ClickByJavascriptRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver> PreExecute { get; set; }
    public Action<IWebDriver> PostExecute { get; set; }
    public By By { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        IJavaScriptExecutor javaScriptExecutor;
        IWebElement element;

        element = driver.FindElement(By);

        javaScriptExecutor = (IJavaScriptExecutor)driver;
        javaScriptExecutor.ExecuteScript("arguments[0].click();", element);
        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}