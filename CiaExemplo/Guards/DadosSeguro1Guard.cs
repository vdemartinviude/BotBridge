using CiaExemplo.PagesStates;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace CiaExemplo.Guards;

public class DadosSeguro1Guard : IGuard<DadosSeguro1, DadosSeguro2>
{
    public uint Priority => 10;

    public bool Condition(Robot robot)
    {
        return true;
    }
}