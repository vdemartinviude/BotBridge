using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chromium;
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
        private IWebDriver _driver { get; set; }

        public string DownloadFolder { get; private set; }

        public Robot()
        {
            var Processes = Process.GetProcesses();

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
            options.AddArgument("--log-level=OFF");

            Log.Logger.Information("Starting the selenium driver");

            _driver = new ChromeDriver(options);

            _driver.Manage().Window.Maximize();
            Log.Logger.Information("Selenium driver started");
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

            if (request.Timeout == null)
            {
                request.Timeout = TimeSpan.FromSeconds(5);
            }

            Log.Information("About to execute {@IRoboRequest}", request);

            request.PreExecute?.Invoke(_driver);

            if (request.DelayBefore.Ticks > 0)
            {
                await Task.Delay(request.DelayBefore);
            }
            var resp = request.Exec(_driver);

            if (resp.Status != RobotResponseStatus.ActionRealizedOk)
            {
                Log.Information("The request was not successfully");
            }

            if (request.DelayAfter.Ticks > 0)
            {
                await Task.Delay(request.DelayAfter);
            }

            request.PostExecute?.Invoke(_driver);

            Log.Information("{@IRoboRequest} Executed", request);
            return resp;
        }
    }
}