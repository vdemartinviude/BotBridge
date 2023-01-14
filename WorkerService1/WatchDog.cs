using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService1;

public class WatchDog : BackgroundService
{
    private readonly ILogger<WatchDog> _logger;
    private AutoResetEvent ev = new AutoResetEvent(false);
    public ManualResetEvent FinalizaMaquina = new ManualResetEvent(false);

    public WatchDog(ILogger<WatchDog> logger)
    {
        _logger = logger;
    }

    public void SetWatchDog()
    {
        ev.Set();
    }

    public void WaitProc(object state, bool timeOut)
    {
        if (timeOut)
        {
            _logger.LogCritical("The Watchdog has been fired!!!!");
            FinalizaMaquina.Set();
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Watch dog service is starting");
        ThreadPool.RegisterWaitForSingleObject(ev, new WaitOrTimerCallback(WaitProc), null, (int)TimeSpan.FromMinutes(15).TotalMilliseconds, false);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Watch dog - Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}