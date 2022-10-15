using StatesAndEvents;
using TheRobot;
using WorkerService1;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<Robot>();
        services.AddSingleton<BaseOrcamento>(x => new BaseOrcamento(Path.Combine(Environment.CurrentDirectory, "JsonExemplo.json")));
        services.AddHostedService<Worker>();
        services.AddHostedService<WatchDog>();
    })
    .Build();

await host.RunAsync();