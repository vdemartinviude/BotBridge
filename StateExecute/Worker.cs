using JsonDocumentsManager;
using Microsoft.Extensions.Hosting;
using StatesAndEvents;
using TheRobot;
using TheStateMachine;
using TheStateMachine.Model;

namespace StateExecute
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly TheMachine _theMachine;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public Worker(ILogger<Worker> logger, TheMachine theMachine, IHostApplicationLifetime applicationLifetime)
        {
            _logger = logger;
            _theMachine = theMachine;
            _applicationLifetime = applicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _theMachine.Build();
            _theMachine.ExecuteMachine();

            while (!stoppingToken.IsCancellationRequested && _theMachine.RobotWorking)
            {
                _logger.LogInformation("Worker running at: {time} and Machine Running = {running}", DateTimeOffset.Now, _theMachine.RobotWorking);
                await Task.Delay(1000, stoppingToken);
            }

            _applicationLifetime.StopApplication();
        }
    }
}