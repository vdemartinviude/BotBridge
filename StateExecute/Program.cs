using JsonDocumentsManager;
using StateExecute;
using System.Reflection;
using TheRobot;
using TheStateMachine;
using TheStateMachine.Helpers;
using TheStateMachine.Model;

var machineInfra = new MachineInfrastructure(
    TheStateMachineHelpers.GetMachineSpecification(Assembly.Load("WordpressStatesAndGuards")),
    new Robot(),
    new InputJsonDocument(Path.Combine(Environment.CurrentDirectory, "JsonDocuments", "InputData.json")),
    new ResultJsonDocument());

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(machineInfra);
        services.AddSingleton<TheMachine>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();