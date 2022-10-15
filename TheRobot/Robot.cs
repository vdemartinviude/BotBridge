using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;
using System.Diagnostics;
using TheRobot.Requests;
using TheRobot.Response;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace TheRobot
{
    public class Robot : IRobot, IDisposable
    {
        public IWebDriver _driver { get; private set; }

        public string DownloadFolder { get; private set; }

        public Robot()
        {
            var Processes = Process.GetProcesses();
            //TODO: Amarrar com o usuário da máquina. Pois há outros chromes rodando na mesma máquina
            Processes.Where(p => p.ProcessName.ToLower().Contains("chrome")).ToList().ForEach(x => x.Kill());
            DownloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "RobotDownloads");
            if (!Directory.Exists(DownloadFolder))
            {
                Directory.CreateDirectory(DownloadFolder);
            }
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/therobot.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()

                .CreateLogger();

            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            ChromeOptions options = new();

            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.default_directory", DownloadFolder);
            //options.AddArgument("--headless");
            //var user_agent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.50 Safari/537.36";
            //options.AddArgument($"user-agent={user_agent}");
            _driver = new ChromeDriver(options);
            _driver.Manage().Window.Maximize();
        }

        public void Dispose()
        {
            _driver?.Quit();
        }

        public async Task<RobotResponse> Execute(IRobotRequest request)
        {
            if (_driver == null)
            {
                throw new Exception("Driver not loaded!");
            }

            Log.Information("About to execute {@IRoboRequest}", request);

            request.PreExecute?.Invoke(_driver);

            if (request.DelayBefore.Ticks > 0)
            {
                await Task.Delay(request.DelayBefore);
            }
            var resp = request.Exec(_driver);

            if (request.DelayAfter.Ticks > 0)
            {
                await Task.Delay(request.DelayAfter);
            }

            request.PostExecute?.Invoke(_driver);

            Log.Debug("{@IRoboRequest} Executed", request);
            return resp;
        }
    }
}