using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using StatesAndEvents;
using TheRobot;
using TheRobot.Requests;

namespace CiaExemplo.PagesStates;

public class MeuCotador : BaseState
{
    public MeuCotador(Robot robot, BaseOrcamento inputdata) : base("MeuCotador", robot, inputdata)
    {
    }

    public override void Execute()
    {
        var click = new ClickRequest()
        {
            By = By.XPath("//input[@id='Acessar']")
        };
        _robot.Execute(click);
    }
}