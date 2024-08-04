using Microsoft.Playwright.NUnit;
using NUnit.Framework.Internal;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace script_template_csharp;

[Parallelizable(ParallelScope.Self)]
public class 測試_瀏覽器特定畫面 : PageTest
{
    public HttpClient _httpClient;
    public ILogger _logger;

    [SetUp]
    public void SetUp()
    {
        _httpClient = new HttpClient();
        _logger = new LoggerFactory().CreateLogger<測試_瀏覽器特定畫面>();
    }
    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }

    [Test]
    public async Task 範例_1_驗證網站有沒有標題()
    {
        // 瀏覽至 Playwright 官方網站
        await Page.GotoAsync("https://playwright.dev");
        string? ans = "Fast and reliable end-to-end testing for modern web apps";

        try
        {
            string? val = await Page.TitleAsync();
            // 使用軟斷言
            Assert.AreEqual(ans, val);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            // 設置重試邏輯
            int retries = 2;
            bool success = false;
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    string? val = await Page.TitleAsync();
                    Assert.AreEqual(ans, val);
                    success = true;
                    break;
                }
                catch (Exception retryException)
                {
                    _logger.LogError($"Retry {i + 1} failed: {retryException.Message}");
                }
            }

            if (!success)
            {
                Assert.Fail(e.Message);
            }
        }
    }

    [Test]
    public async Task 驗證是否存在()
    {
        await Page.GotoAsync("https://example.com");
        Assert.True(await Page.IsVisibleAsync("h1"));
    }

    [Test]
    public async Task 內容驗證()
    {
        var context = await Browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://example.com");
        Assert.AreEqual("Example Domain", await page.TitleAsync());
    }

    [Test]
    public async Task 多網站驗證()
    {
        var page1 = await Browser.NewPageAsync();
        await page1.GotoAsync("https://example.com");

        var page2 = await Browser.NewPageAsync();
        await page2.GotoAsync("https://playwright.dev");

        Assert.AreEqual("Example Domain", await page1.TitleAsync());
        Assert.AreEqual("Fast and reliable end-to-end testing for modern web apps | Playwright", await page2.TitleAsync());
    }
}

