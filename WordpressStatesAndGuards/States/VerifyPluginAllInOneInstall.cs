using JsonDocumentsManager;
using OpenQA.Selenium;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using TheRobot.Requests;
using TheRobot.Response;

namespace WordpressStatesAndGuards.States;

public class VerifyPluginAllInOneInstall : BaseState
{
    public VerifyPluginAllInOneInstall(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("PluginAllInOneInstall", robot, inputdata, resultJson)
    {
    }

    public override void Execute()
    {
        var pluginalreadyinstalled = _robot.ExecuteWithWait(new ElementExistRequest
        {
            By = By.XPath("//div[contains(@class,'wp-menu-name') and contains(text(),'All-in-One WP Migration')]"),
            Timeout = TimeSpan.FromSeconds(2)
        });
        if (pluginalreadyinstalled.Status == RobotResponseStatus.ActionRealizedOk)
        {
            return;
        }

        _robot.ExecuteWithWait(new ClickRequest
        {
            By = By.XPath("//div[contains(text(),'Plugins')]"),
            DelayAfter = TimeSpan.FromSeconds(3)
        });

        _robot.ExecuteWithWait(new ClickRequest
        {
            By = By.XPath("//a[contains(text(),'Adicionar novo') and contains(@class,'page-title-action')]")
        });
        _robot.ExecuteWithWait(new SetTextRequest
        {
            By = By.XPath("//input[@id='search-plugins']"),
            Text = "All-in-one WP Migration",
        });

        RobotResponse buttonInstall;
        do
        {
            buttonInstall = _robot.ExecuteWithWait(new ElementExistRequest
            {
                By = By.XPath("//a[contains(@class,'install-now button') and contains(@data-name,'All-in-One')]"),
                Timeout = TimeSpan.FromSeconds(1)
            });
        } while (buttonInstall.Status != RobotResponseStatus.ActionRealizedOk);

        buttonInstall.WebElement.Click();

        RobotResponse buttonAtivar;
        do
        {
            buttonAtivar = _robot.ExecuteWithWait(new ElementExistRequest
            {
                By = By.XPath("//a[contains(text(),'Ativar') and contains(@data-name,'All-in-One')]"),
                Timeout = TimeSpan.FromSeconds(1)
            });
        } while (buttonAtivar.Status != RobotResponseStatus.ActionRealizedOk);

        buttonAtivar.WebElement.Click();
    }
}