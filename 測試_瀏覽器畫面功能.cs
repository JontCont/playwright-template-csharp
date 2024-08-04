using System.Threading.Tasks;
using Microsoft.Playwright;

namespace script_template_csharp;

[TestFixture]
public class 測試_瀏覽器畫面功能
{
    [Test]
    public async Task 錯誤訊息示範_1_畫面不存在()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            Timeout = 1000,
        });
        var context = await browser.NewContextAsync();

        // Open new page
        var page = await context.NewPageAsync();

        // Go to https://angular.io/
        await page.GotoAsync("https://angular.io/");
        // Click text=Get Started
        try
        {
            var button = await page.WaitForSelectorAsync("text=Learn Angular1", new PageWaitForSelectorOptions { Timeout = 10000 });
            await button!.ClickAsync();
        }
        catch
        {
            Assert.Fail("Get Started button not found");
        }
    }

    [Test]
    public async Task 錯誤訊息示範_2_結果資料不符()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("https://digitalmaster.knsh.com.tw/ju/math/calculator/test_css.html");
        await page.GetByText("６").ClickAsync();
        await page.GetByText("５").ClickAsync();
        await page.GetByText("８").ClickAsync();
        await page.Locator("div:nth-child(3) > div:nth-child(4) > div > img").ClickAsync();
        await page.GetByText("１").ClickAsync();
        await page.GetByText("２").ClickAsync();
        await page.GetByText("３").ClickAsync();
        await page.Locator("div:nth-child(5) > div:nth-child(4) > div > img").ClickAsync();
        await Assertions.Expect(page.Locator("#display-num")).ToContainTextAsync("600.");
    }

    [OneTimeTearDown]
    public void GenerateReport()
    {
        var reportCommand = "npx playwright show-report";
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {reportCommand}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        });
    }
}

