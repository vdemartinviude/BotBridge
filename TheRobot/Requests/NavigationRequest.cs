using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class NavigationRequest : IRobotRequest
{
    public string? Url { get; set; }
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        if (String.IsNullOrEmpty(Url))
        {
            ArgumentNullException argumentNullException = new("Url", "The parameter must not be nulll");
            throw argumentNullException;
        }
        
        try
        {
            driver.Navigate().GoToUrl(Url);
        }
        catch (System.Exception ex)
        {
            return new()
            {
                Status = RobotResponseStatus.ExceptionOccurred,
                ErrorMessage = ex.Message
            };

        }
        
        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };

    }

  
}
