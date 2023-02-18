using JsonDocumentsManager;
using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.PagesStates;

public class ProcessaAlertas : BaseState
{
    public ProcessaAlertas(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("ProcessaAlertas", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        var alertas = await _robot.Execute(new GetElementsListRequest
        {
            By = By.XPath("//div[@role='alert']")
        });
        if (alertas.WebElements == null)
            return;

        //TODO: Register alerts on result.
        foreach (var alert in alertas.WebElements)
        {
            _results.AddResultMessage("Mensagem [CIA]", alert.Text);

            if (alert.Text.Contains("Fator de ajuste com restrição"))
            {
                await _robot.Execute(new SetTextWithKeyDownAndBackSpaceRequest()
                {
                    BackSpaceNumber = 7,
                    By = By.Id("TipoCobertura_FatorAjuste_1"),
                    Text = "10000"
                });
            }
            if (alert.Text.Contains("A cobertura de Peças"))
            {
                await _robot.Execute(new SelectBy2ClicksRequest()
                {
                    By1 = By.XPath("//div[@id='PecasReposicao_1_chosen']//b"),
                    By2 = By.XPath("//div[@id='PecasReposicao_1_chosen']//li[contains(text(),'ORIGINAIS')]")
                });
            }
        }

        await _robot.Execute(new ClickRequest()
        {
            By = By.Id("btnCalcular"),
            Timeout = TimeSpan.FromSeconds(1),
            DelayAfter = TimeSpan.FromSeconds(20)
        });
    }
}