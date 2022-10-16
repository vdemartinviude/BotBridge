using Appccelerate.StateMachine;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace StatesAndEvents;

public class BaseState : IState
{
    public string Name { get; private set; }
    protected readonly Robot _robot;
    protected readonly BaseOrcamento _orcamento;

    public BaseState(string name, Robot robot, BaseOrcamento inputdata)
    {
        Name = name;
        _robot = robot;
        _orcamento = inputdata;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return this.Equals((BaseState)obj);
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode();
    }

    public int CompareTo(object? obj)
    {
        throw new NotImplementedException();
    }

    protected bool Equals(BaseState other)
    {
        return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }

    public void MainExecute(ActiveStateMachine<BaseState, RobotEvents> passiveStateMachine)
    {
        Execute();
        Thread.Sleep(100);
        var waitPageLoadIfNecessary = new WebDriverWait(_robot._driver, TimeSpan.FromSeconds(10))
            .Until(a => a.FindElement(By.XPath("//body")));
        passiveStateMachine.Fire(RobotEvents.NormalTransition);
    }

    public virtual void Execute()
    {
    }
}