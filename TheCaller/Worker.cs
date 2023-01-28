using TheStateMachine;

namespace TheCaller
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly TheMachine _theMachine;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public Worker(ILogger<Worker> logger, TheMachine theMachine, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _theMachine = theMachine;
            _applicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _theMachine.Build();
            _theMachine.ExecuteMachine();

            int i = 0;
            while (!stoppingToken.IsCancellationRequested && _theMachine.Machine!.IsRunning)
            {
                _logger.LogInformation("{i} \tWorker running at: {time}", i++, DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
            if (_theMachine.Machine!.IsRunning)
            {
                _logger.LogInformation("Finalizando a máquina de forma não mascarável...");
                await _theMachine.Machine!.FirePriority(StatesAndEvents.MachineEvents.FinalizeMachine);
                while (_theMachine.Machine.IsRunning)
                {
                    _logger.LogInformation("Waiting the machine stops");
                    await Task.Delay(500, stoppingToken);
                }
            }
            _logger.LogInformation("A máquina foi finalizada!");
            _applicationLifetime.StopApplication();
        }
    }
}