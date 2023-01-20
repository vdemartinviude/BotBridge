using Liberty.PagesStates;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace Liberty.Guards;

public class ErrorLoginGuard : IGuard<ErroLogin>
{
    public bool Condition(Robot robot)
    {
        return true;
    }
}