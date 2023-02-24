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

namespace StatesForMachineTest.States;

public class JustaState : BaseState
{
    public override async Task Execute(CancellationToken token)
    {
        await _robot.Execute(new NavigationRequest
        {
            Url = "http://www.uol.com.br"
        });
        await _robot.Execute(new WaitElementExistsOrVanishRequest
        {
            By = By.XPath("dskajdklasdjaklsdja"),
            CancellationToken = token,
            WaitVanish = true
        });
    }

    public override TimeSpan StateTimeout => TimeSpan.FromSeconds(20);

    public JustaState(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("JustaState", robot, inputdata, resultJson)
    {
    }
}