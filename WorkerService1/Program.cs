using JsonDocumentsManager;
using StatesAndEvents;
using TheRobot;
using WorkerService1;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices(services =>
    {
        services.AddSingleton<Robot>();
        services.AddSingleton<BaseOrcamento>(x => new BaseOrcamento(Path.Combine(Environment.CurrentDirectory, "JsonExemplo.json")));
        services.AddSingleton<WatchDog>();
        services.AddSingleton<ResultJsonDocument>();
        services.AddHostedService<Worker>();
        services.AddSingleton<CancellationTokenSource>();
    })
    .Build();
CancellationTokenSource source = host.Services.GetService<CancellationTokenSource>()!;
CancellationToken watchDogCancelation = source.Token;
await host.Services.GetService<WatchDog>()!.StartAsync(watchDogCancelation);
await host.RunAsync(watchDogCancelation);