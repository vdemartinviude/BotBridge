using JsonDocumentsManager;
using OpenQA.Selenium;
using StatesAndEvents;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.PagesStates;

public class LibertyAutoPerfil : BaseState
{
    public LibertyAutoPerfil(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("LibertyAutoPerfil", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        await _robot.Execute(new ClickByJavascriptRequest()
        {
            By = By.XPath("//label[contains(text(),'31111')]//.."),
            DelayBefore = TimeSpan.FromSeconds(1),
            DelayAfter = TimeSpan.FromSeconds(10)
        });
    }
}