using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobot.Response;

public enum RobotResponseStatus
{
    ActionRealizedOk,
    ElementNotFound,
    ExceptionOccurred
}
public class RobotResponse
{
    public RobotResponseStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public IWebElement? Result { get; set; }    
}
