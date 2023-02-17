using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using WordpressStatesAndGuards.States;

namespace WordpressStatesAndGuards.Guards;

public class WpJustConfiguredGuard : IGuard<WPInitialConfiguration, WPLogin>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        return true;
    }
}