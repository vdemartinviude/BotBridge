using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using WordpressStatesAndGuards.States;

namespace WordpressStatesAndGuards.Guards;

public class LoginFailFinalGuard : IGuard<StopRobotLoginError>
{
    public bool Condition(Robot robot)
    {
        throw new NotImplementedException();
    }
}