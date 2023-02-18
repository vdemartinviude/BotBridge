using JsonDocumentsManager;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace StatesForMachineTest.States;

public class JustaState : BaseState
{
    public override async Task Execute(CancellationToken token)
    {
        await Task.Delay(TimeSpan.FromSeconds(3), token);
    }

    public override TimeSpan StateTimeout => TimeSpan.FromSeconds(20);

    public JustaState(Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson) : base("JustaState", robot, inputdata, resultJson)
    {
    }
}