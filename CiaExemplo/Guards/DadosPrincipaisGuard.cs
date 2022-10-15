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

namespace CiaExemplo.Guards;

public class DadosPrincipaisGuard : IGuard<DadosPrincipais, DadosSeguro1>
{
    public bool Condition(Robot robot)
    {
        var wait = new WebDriverWait(robot._driver, TimeSpan.FromSeconds(30))
            .Until(ExpectedConditions.ElementToBeClickable(By.Id("CodigoCodigoEnderecamentoPostalPernoite")));
        return true;
    }
}