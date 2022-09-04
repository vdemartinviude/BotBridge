using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using TestRobot.Fixtures;
using TheRobot;
using TheRobot.Requests;
using TheRobot.Response;

namespace TestRobot;

public class RobotTests : IClassFixture<RobotFixture>
{
    private readonly RobotFixture _robotfixture;
    private readonly Robot _robot;

    public RobotTests(RobotFixture robotfixture)
    {
        _robotfixture = robotfixture;
        _robot = _robotfixture.host.Services.GetRequiredService<Robot>();
    }

    [Theory]
    [InlineData("http://www.uol.com.br")]
    [InlineData("http://www.google.com.br")]
    public void RobotCanNavigate(string url)
    {
        var req = new NavigationRequest()
        {
            Url = url
        };

        Assert.Equal<RobotResponseStatus>(RobotResponseStatus.ActionRealizedOk, _robot.Execute(req).Result.Status);
    }

    [Fact]
    public void RobotCatchsTheExceptionWhenCantNavigate()
    {
        var req = new NavigationRequest()
        {
            Url = "http://www.fjdslkfdjls.csfdsf"
        };
        Assert.Equal<RobotResponseStatus>(RobotResponseStatus.ExceptionOccurred, _robot.Execute(req).Result.Status);
    }

    [Fact]
    public void ThowNullExceptionWhenNavigateToNullUrl()
    {
        var req = new NavigationRequest();
        Assert.ThrowsAsync<ArgumentNullException>(() => _robot.Execute(req));
    }

    [Fact]
    public async void RobotCanClick()
    {
        // Arrange
        var req = new NavigationRequest()
        {
            Url = "https://rpachallenge.com"
        };
        await _robot.Execute(req);

        var req2 = new ClickRequest()
        {
            DelayBefore = TimeSpan.FromSeconds(5),
            By = By.XPath("//a[contains(text(),'Download')]"),
            DelayAfter = TimeSpan.FromSeconds(30),
        };
        await _robot.Execute(req2);
    }

    [Fact]
    public async void TestPreExecute()
    {
        // Arrange
        var req = new NavigationRequest()
        {
            Url = "http://www.g1.com",
            PreExecute = ((driver) => _robotfixture.logger.Info(driver.Url))
        };
        // Act
        await _robot.Execute(req);
    }

    [Fact]
    public async void EnsureNullExceptionOnNullByGetElement()
    {
        var req = new GetElement()
        {
            Timeout = new TimeSpan(0, 0, 10)
        };

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _robot.Execute(req));
    }

    [Fact]
    public async void EnsureNullExceptionOnNullTimeoutGetElement()
    {
        var req = new GetElement()
        {
            By = By.XPath("")
        };
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _robot.Execute(req));
    }

    [Fact]
    public async void EnsureCanGetElement()
    {
        // Arrange
        var req = new NavigationRequest()
        {
            Url = "https://rpachallenge.com"
        };
        await _robot.Execute(req);

        var req2 = new GetElement()
        {
            By = By.XPath("//div[contains(@class,'instructionsText')][1]"),
            Timeout = new TimeSpan(0, 0, 5)
        };
        var resp = await _robot.Execute(req2);

        Assert.Contains("The goal of this challenge", resp.WebElement.Text);
    }

    [Fact]
    public async void EnsuseTimeoutOnDownloadFile()
    {
        var req = new NavigationRequest()
        {
            Url = "https://rpachallenge.com"
        };
        await _robot.Execute(req);

        var req2 = new ClickRequest()
        {
            By = By.XPath("//a[contains(text(),'Download')]")
        };
        await _robot.Execute(req2);

        var req3 = new WaitForDownload()
        {
            Folder = _robot.DownloadFolder,
            FileTypes = new List<string>()
            {
                {"*.doc" }
            },
            Timeout = new TimeSpan(0, 1, 0)
        };

        var resp = await _robot.Execute(req3);

        Assert.Equal<RobotResponseStatus>(RobotResponseStatus.TimedOut, resp.Status);
    }

    [Fact]
    public async void EnsureCanDownloadFile()
    {
        var req = new NavigationRequest()
        {
            Url = "https://rpachallenge.com"
        };
        await _robot.Execute(req);

        var req2 = new ClickRequest()
        {
            By = By.XPath("//a[contains(text(),'Download')]")
        };
        await _robot.Execute(req2);

        var req3 = new WaitForDownload()
        {
            Folder = _robot.DownloadFolder,
            FileTypes = new List<string>()
            {
                {"*.xlsx" }
            }
        };

        var resp = await _robot.Execute(req3);

        Assert.Equal<RobotResponseStatus>(RobotResponseStatus.ActionRealizedOk, resp.Status);
    }
}