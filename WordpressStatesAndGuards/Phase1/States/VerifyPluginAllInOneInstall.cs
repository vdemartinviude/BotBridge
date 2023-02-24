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
    public override TimeSpan StateTimeout => TimeSpan.FromMinutes(60);

    public VerifyPluginAllInOneInstall(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("PluginAllInOneInstall", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        var pluginalreadyinstalled = await _robot.Execute(new ElementExistRequest
        {
            By = By.XPath("//div[contains(@class,'wp-menu-name') and contains(text(),'All-in-One WP Migration')]"),
            Timeout = TimeSpan.FromSeconds(2)
        });
        if (pluginalreadyinstalled.Status == RobotResponseStatus.ActionRealizedOk)
        {
            return;
        }

        await _robot.Execute(new ClickRequest
        {
            By = By.XPath("//div[contains(text(),'Plugins')]"),
            DelayAfter = TimeSpan.FromSeconds(3)
        });

        await _robot.Execute(new ClickRequest
        {
            By = By.XPath("//a[contains(text(),'Adicionar novo') and contains(@class,'page-title-action')]")
        });
        await _robot.Execute(new SetTextRequest
        {
            By = By.XPath("//input[@id='search-plugins']"),
            Text = "All-in-one WP Migration",
        });

        RobotResponse buttonInstall = await _robot.Execute(new WaitElementExistsOrVanishRequest
        {
            CancellationToken = token,
            By = By.XPath("//a[contains(@class,'install-now button') and contains(@data-name,'All-in-One')]"),
        });

        //do
        //{
        //    buttonInstall = await _robot.Execute(new ElementExistRequest
        //    {
        //        By = By.XPath("//a[contains(@class,'install-now button') and contains(@data-name,'All-in-One')]"),
        //        Timeout = TimeSpan.FromSeconds(1)
        //    });
        //} while (buttonInstall.Status != RobotResponseStatus.ActionRealizedOk);

        buttonInstall.WebElement!.Click();

        RobotResponse buttonAtivar = await _robot.Execute(new WaitElementExistsOrVanishRequest
        {
            CancellationToken = token,
            By = By.XPath("//a[contains(text(),'Ativar') and contains(@data-name,'All-in-One')]"),
        });
        //do
        //{
        //    buttonAtivar = await _robot.Execute(new ElementExistRequest
        //    {
        //        By = By.XPath("//a[contains(text(),'Ativar') and contains(@data-name,'All-in-One')]"),
        //        Timeout = TimeSpan.FromSeconds(1)
        //    });
        //} while (buttonAtivar.Status != RobotResponseStatus.ActionRealizedOk);

        buttonAtivar.WebElement!.Click();
    }
}