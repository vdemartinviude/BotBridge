using JsonDocumentsManager;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using StatesAndEvents;
using TheRobot;
using TheRobot.Requests;

namespace Liberty.PagesStates
{
    public class AcessaCotacaoAuto : BaseState
    {
        public AcessaCotacaoAuto(Robot robot, InputJsonDocument baseOrcamento, ResultJsonDocument resultJson) : base("AcessaCotacaoAuto", robot, baseOrcamento, resultJson)
        {
        }

        public override async Task Execute(CancellationToken token)
        {
            await _robot.Execute(new ChangeWindowByClickRequest
            {
                By = By.XPath("//a[contains(text(),'Cotar')]"),
                Timeout = TimeSpan.FromSeconds(1),
            });
        }
    }
}