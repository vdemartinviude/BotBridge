using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using StatesAndEvents;
using TheRobot;
using TheRobot.Requests;

namespace CiaExemplo.PagesStates
{
    public class AcessaCotacaoAuto : BaseState
    {
        public AcessaCotacaoAuto(Robot robot, BaseOrcamento baseOrcamento) : base("AcessaCotacaoAuto", robot, baseOrcamento)
        {
        }

        public override void Execute()
        {
            string originalWindow = _robot._driver.CurrentWindowHandle;

            var request = new ClickRequest()
            {
                By = By.XPath("//a[contains(text(),'Cotar')]"),
                Timeout = TimeSpan.FromSeconds(1),
                DelayAfter = TimeSpan.FromSeconds(3),
            };
            _robot.Execute(request).Wait();

            var wait = new WebDriverWait(_robot._driver, TimeSpan.FromSeconds(1)).Until(wd => wd.WindowHandles.Count == 2);
            foreach (string window in _robot._driver.WindowHandles)
            {
                if (originalWindow != window)
                {
                    _robot._driver.SwitchTo().Window(window);
                    break;
                }
            }
        }
    }
}