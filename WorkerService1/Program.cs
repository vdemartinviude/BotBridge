using StatesAndEvents;
using TheRobot;
using WorkerService1;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<Robot>();
        services.AddSingleton<BaseOrcamento>(x => new BaseOrcamento(Path.Combine(Environment.CurrentDirectory, "JsonExemplo.json")));
        services.AddSingleton<WatchDog>();
        services.AddHostedService<Worker>();
    })
    .Build();
await host.Services.GetService<WatchDog>().StartAsync(new CancellationToken());
await host.RunAsync();