using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using StateMachineTests.Fixtures;
using TheStateMachine;

namespace StateMachineTests;

public class TheStateMachineTests : IClassFixture<StateMachineFixture>
{
    private readonly StateMachineFixture _machineFixture;
    private readonly ILogger<StateMachineFixture> _logger;
    private readonly TheMachine _machine;

    public TheStateMachineTests(StateMachineFixture machineFixture)
    {
        _machineFixture = machineFixture;
        _logger = _machineFixture.host.Services.GetRequiredService<ILogger<StateMachineFixture>>();
        _machine = _machineFixture.host.Services.GetRequiredService<TheMachine>();
    }

    [Fact]
    public void Test1()
    {
        var obj = new
        {
            Hora = System.DateTime.Now,
            Msg = "Vinicius"
        };
        _machine.Build();
        _logger.LogInformation("Teste {@obj}", obj);
    }
}