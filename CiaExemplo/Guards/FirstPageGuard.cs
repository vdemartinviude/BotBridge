using CiaExemplo.PagesStates;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace CiaExemplo.Guards;

public class FirstPageGuard : IGuard<FirstPage, FazLogin>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        robot.Execute(new WaitAndMoveToElementClickableRequest
        {
            Timeout = TimeSpan.FromSeconds(10),
            By = By.XPath("//input[@name='username']")
        }).Wait();

        return true;
    }
}