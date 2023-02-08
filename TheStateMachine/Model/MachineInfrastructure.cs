using JsonDocumentsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace TheStateMachine.Model;

public class MachineInfrastructure
{
    public MachineSpecification MachineSpecification { get; private set; }
    public Robot Robot { get; private set; }
    public InputJsonDocument InputJsonDocument { get; private set; }
    public ResultJsonDocument ResultJsonDocument { get; private set; }

    public MachineInfrastructure(MachineSpecification machineSpecification, Robot robot, InputJsonDocument inputJsonDocument, ResultJsonDocument resultJsonDocument)
    {
        MachineSpecification = machineSpecification;
        Robot = robot;
        InputJsonDocument = inputJsonDocument;
        ResultJsonDocument = resultJsonDocument;
    }
}