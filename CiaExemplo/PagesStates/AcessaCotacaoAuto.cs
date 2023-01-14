using JsonDocumentsManager;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using StatesAndEvents;
using TheRobot;
using TheRobot.Requests;

namespace CiaExemplo.PagesStates
{
    public class AcessaCotacaoAuto : BaseState
    {
        public AcessaCotacaoAuto(Robot robot, BaseOrcamento baseOrcamento, ResultJsonDocument resultJson) : base("AcessaCotacaoAuto", robot, baseOrcamento, resultJson)
        {
        }

        public override void Execute()
        {
            _robot.Execute(new ChangeWindowByClick
            {
                By = By.XPath("//a[contains(text(),'Cotar')]"),
                Timeout = TimeSpan.FromSeconds(1),
            }).Wait();
        }
    }
}