// See https://aka.ms/new-console-template for more information
using ExcelDataReader;
using System.Data;
using OpenQA.Selenium;
using TheRobot;
using TheRobot.Requests;

internal class Program
{
    private static async Task Main(string[] args)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        Robot robot = new();
        var navigate1 = new NavigationRequest
        {
            Url = "https://rpachallenge.com",
            DelayAfter = TimeSpan.FromSeconds(5),
        };
        robot.Execute(navigate1).Wait();
        var req2 = new ClickRequest()
        {
            By = By.XPath("//a[contains(text(),'Download')]")
        };
        await robot.Execute(req2);

        var req3 = new WaitForDownload()
        {
            Folder = robot.DownloadFolder,
            FileTypes = new List<string>()
            {
                {"*.xlsx" }
            }
        };

        var resp = await robot.Execute(req3);
        DataSet result;
        using (var stream = new FileStream(resp.Data, FileMode.Open, FileAccess.Read))
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
            result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (x => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                })
            });
        }

        foreach (DataRow row in result.Tables[0].Rows)
        {
            var set1 = new SetTextRequest()
            {
                By = By.XPath("//label[text()='Role in Company']/../input"),
                Text = row["Role in Company"].ToString()
            };
            robot.Execute(set1).Wait();
        }
        robot.Dispose();

        Console.WriteLine("Hello, World!");
    }
}