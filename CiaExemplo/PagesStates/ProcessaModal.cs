using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace CiaExemplo.PagesStates;

public class ProcessaModal : BaseState
{
    public ProcessaModal(Robot robot, BaseOrcamento inputdata) : base("ProcessaModal", robot, inputdata)
    {
    }

    public override void Execute()
    {
        //TODO: Register the modal message on result
        base.Execute();
    }
}