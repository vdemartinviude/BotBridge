using JsonDocumentsManager;
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

        public Worker(ILogger<Worker> logger, TheMachine theMachine)
        {
            _logger = logger;
            _theMachine = theMachine;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _theMachine.Build();
            _theMachine.ExecuteMachine();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}