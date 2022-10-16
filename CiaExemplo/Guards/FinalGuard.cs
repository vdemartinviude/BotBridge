using CiaExemplo.PagesStates;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace CiaExemplo.Guards;

public class FinalGuard : IGuard<ProcessaAlertas>
{
    public bool Condition(Robot robot)
    {
        return true;
    }
}