using JsonDocumentsManager;
using System.Reflection;
using TheCaller;
using TheRobot;
using TheStateMachine;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<Robot>();
        services.AddSingleton(x => new InputJsonDocument(Path.Combine(Environment.CurrentDirectory, "InputDocuments", "JsonExemplo.json")));
        services.AddSingleton<ResultJsonDocument>();
        services.AddSingleton(x => TheStateMachine.Helpers.TheStateMachineHelpers.GetMachineSpecification(Assembly.Load("Liberty")));
        services.AddSingleton<TheMachine>();
    })
    .Build();

await host.RunAsync();