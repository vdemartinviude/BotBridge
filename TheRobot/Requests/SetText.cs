using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Helpers;
using TheRobot.Response;

namespace TheRobot.Requests;

public class SetText : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public By? By { get; set; }
    public string? Text { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        if (By == null)
        {
            throw new ArgumentNullException(nameof(By));
        }
        if (Text == null)
        {
            throw new ArgumentNullException(nameof(Text));
        }
        if (driver == null)
        {
            throw new ApplicationException("WebDriver error");
        }
        IWebElement webElement = null;
        try
        {
            RobotHelpers.DoOrTimeout(() => webElement = driver.FindElement(By), new TimeSpan(0, 0, 2)).Wait();
        }
        catch (Exception ex)
        {
            return new RobotResponse()
            {
                Status = RobotResponseStatus.ElementNotFound
            };
        }
        Task.Delay(100).Wait();
        var rnd = new Random();
        webElement?.Click();
        Task.Delay(100).Wait();
        foreach(var c in Text)
        {
            webElement?.SendKeys(c.ToString());
            Task.Delay(rnd.Next(100, 200)).Wait();
        }
        return new RobotResponse()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
        
    }
}
