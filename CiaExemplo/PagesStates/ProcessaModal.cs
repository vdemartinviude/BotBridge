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

public class ProcessaModal : BaseState
{
    public ProcessaModal(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("ProcessaModal", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        //TODO: Register the modal message on result
        await _robot.Execute(new ChangeFrameRequest
        {
            By = By.XPath("//div[@id='modal-body-cotacao']//iframe"),
            Timeout = TimeSpan.FromSeconds(2)
        });

        var title = _robot.Execute(new GetElementRequest()
        {
            By = By.XPath("//div[@class='form-title']"),
            Timeout = TimeSpan.FromSeconds(2)
        }).Result.WebElement!.Text;

        var conteudo = _robot.Execute(new GetElementRequest()
        {
            By = By.XPath("//div[@class='conteudo-modal']"),
            Timeout = TimeSpan.FromSeconds(2)
        }).Result.WebElement!.Text;

        _results.AddResultMessage("Mensagem [CIA]", conteudo, title);

        await _robot.Execute(new SwitchToDefaultFrameRequest());

        await _robot.Execute(new ClickRequest()
        {
            By = By.XPath("//button[text()='Fechar' and @id='btnConfirmar']"),
            Timeout = TimeSpan.FromSeconds(2)
        });

        await _results.SaveDocument("Resultados.json");
    }
}