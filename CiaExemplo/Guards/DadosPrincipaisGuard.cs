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

public class DadosPrincipaisGuard : IGuard<DadosPrincipais, DadosSeguro1>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        robot.Execute(new WaitElementBeClickableRequest
        {
            By = By.Id("CodigoCodigoEnderecamentoPostalPernoite"),
            Timeout = TimeSpan.FromSeconds(30)
        }).Wait();
        return true;
    }
}