using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class SetTextWithJs : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public By By { get; set; }
    public string Text { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        try
        {
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
            var element = driver.FindElement(By);
            javaScriptExecutor.ExecuteScript($"arguments[0].value='{Text}';", element);
        }
        catch (Exception e)
        {
            return new()
            {
                Status = RobotResponseStatus.ElementNotFound
            };
        }
        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}