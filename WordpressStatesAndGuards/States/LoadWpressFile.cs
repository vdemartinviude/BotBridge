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

namespace WordpressStatesAndGuards.States;

public class LoadWpressFile : BaseState
{
    public LoadWpressFile(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("LoadWpressFile", robot, inputdata, resultJson)
    {
    }

    public override void Execute()
    {
        _robot.ExecuteWithWait(new ClickRequest
        {
            By = By.XPath("//div[contains(@class,'wp-menu-name') and contains(text(),'All-in-One WP Migration')]")
        });

        _robot.ExecuteWithWait(new ClickRequest
        {
            By = By.XPath("//a[contains(text(),'Importar') and contains(@href,'wm_import')]")
        });

        _robot.ExecuteWithWait(new ClickRequest
        {
            By = By.XPath("//div[contains(@class,'button-main')]")
        });

        _robot.ExecuteWithWait(new UploadFileByInputSelectRequest
        {
            InputSelectBy = By.Id("ai1wm-select-file"),
            FilePath = _inputData.GetStringData("$.WpressFilePath")
        });
    }
}