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

    public override void Execute()
    {
        var clickjava = new ClickByJavascriptRequest()
        {
            By = By.XPath("//label[contains(text(),'31111')]//.."),
            DelayBefore = TimeSpan.FromSeconds(1),
            DelayAfter = TimeSpan.FromSeconds(10)
        };
        _robot.Execute(clickjava).Wait();
    }
}