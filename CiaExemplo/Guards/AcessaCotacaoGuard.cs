using CiaExemplo.PagesStates;
using OpenQA.Selenium;
using StatesAndEvents;
using StatesAndEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace CiaExemplo.Guards;

public class AcessaCotacaoGuard : IGuard<AcessaCotacaoAuto, SelecaoCanal>
{
    public bool Condition(Robot robot)
    {
        return WaitTransitions.ElementSelectable(By.Id("ddlBranch"), robot);
    }
}