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

    public override void Execute()
    {
        var SelectRequest = new SelectTextRequest
        {
            By = By.Id("ddlBranch"),
            Text = _inputData.GetStringData(JsonPath.Parse("$.Ramo")),
            DelayAfter = TimeSpan.FromSeconds(2),
            DelayBefore = TimeSpan.FromSeconds(3),
        };
        _robot.Execute(SelectRequest).Wait();
    }
}