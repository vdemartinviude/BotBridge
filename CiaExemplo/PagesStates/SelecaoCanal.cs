using Json.Path;
using JsonDocumentsManager;
using OpenQA.Selenium;
using StatesAndEvents;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.PagesStates;

public class SelecaoCanal : BaseState
{
    public SelecaoCanal(Robot robot, InputJsonDocument baseOrcamento, ResultJsonDocument resultJson) : base("SelecaoCanal", robot, baseOrcamento, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        await _robot.Execute(new SelectTextRequest
        {
            By = By.Id("ddlBranch"),
            Text = _inputData.GetStringData(JsonPath.Parse("$.Ramo")),
            DelayAfter = TimeSpan.FromSeconds(2),
            DelayBefore = TimeSpan.FromSeconds(3),
        });
    }
}