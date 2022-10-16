using Json.Path;
using OpenQA.Selenium;
using StatesAndEvents;
using TheRobot;
using TheRobot.Requests;

namespace CiaExemplo.PagesStates;

public class SelecaoCanal : BaseState
{
    public SelecaoCanal(Robot robot, BaseOrcamento baseOrcamento) : base("SelecaoCanal", robot, baseOrcamento)
    {
    }

    public override void Execute()
    {
        var SelectRequest = new SelectText
        {
            By = By.Id("ddlBranch"),
            Text = _orcamento.GetStringData(JsonPath.Parse("$.Ramo")),
            DelayAfter = TimeSpan.FromSeconds(2),
            DelayBefore = TimeSpan.FromSeconds(3),
        };
        _robot.Execute(SelectRequest).Wait();
    }
}