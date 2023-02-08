using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using Serilog;
using TheStateMachine;
using System.Reflection;
using JsonDocumentsManager;
using TheStateMachine.Model;

namespace StateMachineTests.Fixtures;

public class StateMachineFixture : IDisposable
{
    public IHost host;

    public StateMachineFixture()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("log.txt")
            .CreateLogger();

        host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(x => new MachineInfrastructure(
                    TheStateMachine.Helpers.TheStateMachineHelpers.GetMachineSpecification(Assembly.Load("Liberty")),
                    new Robot(),
                    new InputJsonDocument(Path.Combine(Environment.CurrentDirectory, "InputDocuments", "JsonExemplo.json")),
                    new ResultJsonDocument()));
                services.AddSingleton<TheMachine>();
            })
            .UseSerilog()
            .Build();
    }

    public void Dispose()
    {
        host.Services.GetRequiredService<Robot>().Dispose();
    }
}