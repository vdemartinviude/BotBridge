using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using TheRobot;

namespace TestRobot.Fixtures;

public class RobotFixture : IDisposable
{
    public IHost host;
    public ILogger logger;

    public RobotFixture()
    {
        var target = new FileTarget()
        {
            FileName = @"C:\tmp\RobotLog.txt"
        };
        var config = new LoggingConfiguration();
        config.AddRuleForAllLevels(target);

        logger = NLog.LogManager.Setup().LoadConfiguration(config).GetCurrentClassLogger();
        logger.Info("Starting Fixtures");
        host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<Robot>();
            }).UseNLog().Build();
    }

    public void Dispose()
    {
        host.Services.GetRequiredService<Robot>().Dispose();
        logger.Info("Tchau");
    }
}