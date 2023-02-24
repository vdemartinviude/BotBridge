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

public class LoadWpressFile : BaseState
{
    public override TimeSpan StateTimeout => TimeSpan.FromMinutes(60);

    public LoadWpressFile(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("LoadWpressFile", robot, inputdata, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        await _robot.Execute(new ClickRequest
        {
            By = By.XPath("//div[contains(@class,'wp-menu-name') and contains(text(),'All-in-One WP Migration')]")
        });

        await _robot.Execute(new ClickRequest
        {
            By = By.XPath("//a[contains(text(),'Importar') and contains(@href,'wm_import')]")
        });

        await _robot.Execute(new ClickRequest
        {
            By = By.XPath("//div[contains(@class,'button-main')]")
        });

        await _robot.Execute(new UploadFileByInputSelectRequest
        {
            InputSelectBy = By.Id("ai1wm-select-file"),
            FilePath = _inputData.GetStringData("$.WpressFilePath")
        });

        RobotResponse resp;
        resp = await _robot.Execute(new WaitElementExistsOrVanishRequest
        {
            By = By.XPath("//button[contains(text(),'Continuar') and @class='ai1wm-button-green']"),
            CancellationToken = token
        });

        //do
        //{
        //    resp = await _robot.Execute(new ElementExistRequest
        //    {
        //        By = By.XPath("//button[contains(text(),'Continuar') and @class='ai1wm-button-green']"),
        //        Timeout = TimeSpan.FromSeconds(1)
        //    });
        //    token.ThrowIfCancellationRequested();
        //} while (resp.Status != RobotResponseStatus.ActionRealizedOk);

        resp.WebElement!.Click();

        await _robot.Execute(new WaitElementExistsOrVanishRequest
        {
            By = By.XPath("//p[contains(text(),'Restaurando')]"),
            CancellationToken = token
        });
        //do
        //{
        //    resp = await _robot.Execute(new ElementExistRequest
        //    {
        //        By = By.XPath("//p[contains(text(),'Restaurando')]"),
        //        Timeout = TimeSpan.FromSeconds(1)
        //    });
        //} while (resp.Status != RobotResponseStatus.ActionRealizedOk);

        resp = await _robot.Execute(new WaitElementExistsOrVanishRequest
        {
            By = By.XPath("//button[contains(text(),'Finalizar')]"),
            CancellationToken = token
        });
        //do
        //{
        //    resp = await _robot.Execute(new ElementExistRequest
        //    {
        //        By = By.XPath("//button[contains(text(),'Finalizar')]"),
        //        Timeout = TimeSpan.FromSeconds(1)
        //    });
        //} while (resp.Status != RobotResponseStatus.ActionRealizedOk);

        resp.WebElement!.Click();
    }
}