using JsonDocumentsManager;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using StatesAndEvents;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.PagesStates;

public class MeuCotador : BaseState
{
    public MeuCotador(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("MeuCotador", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        await _robot.Execute(new ClickRequest()
        {
            By = By.XPath("//input[@id='Acessar']")
        });
    }
}