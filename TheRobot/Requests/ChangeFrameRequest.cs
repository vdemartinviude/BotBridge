using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class ChangeFrameRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver> PreExecute { get; set; }
    public Action<IWebDriver> PostExecute { get; set; }
    public By By { get; set; }
    public TimeSpan Timeout { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        if (Timeout == TimeSpan.Zero)
        {
            Timeout = TimeSpan.FromSeconds(4);
        }
        try
        {
            var wait = new WebDriverWait(driver, Timeout);
            var iframe = wait.Until(e => e.FindElement(By));
            driver.SwitchTo().Frame(iframe);
        }
        catch (Exception ex)
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