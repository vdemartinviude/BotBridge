using Appccelerate.StateMachine;
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

namespace Liberty.PagesStates;

public class FirstPage : BaseState
{
    public FirstPage(Robot robot, InputJsonDocument baseOrcamento, ResultJsonDocument resultJson) : base("FirstPage", robot, baseOrcamento, resultJson)
    {
    }

    public override async Task Execute(CancellationToken token)
    {
        await _robot.Execute(new NavigationRequest()
        {
            Url = "https://novomeuespacocorretor.libertyseguros.com.br/"
        });
        await _robot.Execute(new ClickRequest()
        {
            By = By.XPath("//a[contains(text(),'Entrar no novo')]"),
            Timeout = new TimeSpan(0, 0, 5)
        });
    }
}