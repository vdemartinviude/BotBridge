using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace StateMachineTests.Fixtures;

public class StateMachineFixture : IDisposable
{
    public IHost host;

    public StateMachineFixture()
    {
        host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<Robot>();
            }).Build();
    }

    public void Dispose()
    {
        host.Services.GetRequiredService<Robot>().Dispose();
    }
}