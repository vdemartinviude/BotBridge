using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class SetText : IRobotRequest
{
    public TimeSpan DelayBefore { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public TimeSpan DelayAfter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action<IWebDriver>? PreExecute { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action<IWebDriver>? PostExecute { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public RobotResponse Exec(IWebDriver driver)
    {
        throw new NotImplementedException();
    }
}
