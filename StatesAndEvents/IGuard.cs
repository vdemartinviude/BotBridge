using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace StatesAndEvents;

public enum RobotEvents
{
    NormalTransition,
    AbortTransition
}

public interface IGuard<TCurrentState, TNextState> where TCurrentState : BaseState where TNextState : BaseState
{
    public abstract bool Condition(Robot robot);
}

public interface IGuard<TFinalState> where TFinalState : BaseState
{
    public abstract bool Condition(Robot robot);
}