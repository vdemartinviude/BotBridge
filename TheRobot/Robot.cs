using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;
using TheRobot.Requests;
using TheRobot.Response;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace TheRobot
{
    public class Robot : IRobot, IDisposable
    {
       
        private IWebDriver? _driver = null;

        public Robot() 
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/therobot.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            new DriverManager().SetUpDriver(new ChromeConfig());
            ChromeOptions options = new();
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.default_directory", "C:\\tmp");
            _driver = new ChromeDriver(options);

        }




        public void Dispose()
        {
            _driver?.Quit();
        }

        
        public async Task<RobotResponse> Execute(IRoboRequest request)
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

            request.PreExecute?.Invoke(_driver);

            Log.Debug("{@IRoboRequest} Executed", request);
            return resp;
        }
    }
}