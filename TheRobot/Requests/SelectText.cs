using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class SelectText : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver> PreExecute { get; set; }
    public Action<IWebDriver> PostExecute { get; set; }
    public string Text { get; set; }
    public By By { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        if (string.IsNullOrEmpty(Text))
        {
            throw new ArgumentNullException("text");
        }
        try
        {
            IWebElement selectElement = new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(x => x.FindElement(By));
            var selectObject = new SelectElement(selectElement);
            selectObject.SelectByText(Text);
        }
        catch (OpenQA.Selenium.WebDriverTimeoutException ex)
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