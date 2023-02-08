using JsonDocumentsManager;
using System.Reflection;
using TheCaller;
using TheRobot;
using TheStateMachine;
using TheStateMachine.Model;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton(x => new MachineInfrastructure(
            TheStateMachine.Helpers.TheStateMachineHelpers.GetMachineSpecification(Assembly.Load("Liberty")),
            new Robot(),
            new InputJsonDocument(Path.Combine(Environment.CurrentDirectory, "InputDocuments", "JsonExemplo.json")),
            new ResultJsonDocument()));
        services.AddSingleton<TheMachine>();
    })
    .Build();

await host.RunAsync();