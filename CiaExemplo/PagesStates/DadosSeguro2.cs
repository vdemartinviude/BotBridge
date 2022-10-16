using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace CiaExemplo.PagesStates;

public class DadosSeguro2 : BaseState
{
    public DadosSeguro2(Robot robot, BaseOrcamento inputdata) : base("DadosSeguro2", robot, inputdata)
    {
    }

    public override void Execute()
    {
        base.Execute();
    }
}