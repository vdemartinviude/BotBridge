﻿using Liberty.PagesStates;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.Guards;

public class MeuCotadorGuard : IGuard<MeuCotador, LibertyAutoPerfil>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        var element = robot.Execute(new ElementExistRequest
        {
            Timeout = TimeSpan.FromSeconds(5),
            By = By.XPath("//h1[contains(text(),'Meu Cotador')]")
        }).Result;
        if (element.Status == TheRobot.Response.RobotResponseStatus.ActionRealizedOk && element.WebElement!.Displayed)
            return true;
        return false;
    }
}