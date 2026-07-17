using System.Diagnostics;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BiotLabTests.Selenium;

[TestClass]
[TestCategory("Selenium")]
public sealed class SeleniumSmokeTests
{
    private static Process? _applicationProcess;
    private static string _baseUrl = "https://localhost:7257";
    private static string? _webProjectDirectory;
    private IWebDriver? _driver;
    private WebDriverWait? _wait;

    public TestContext TestContext { get; set; } = null!;

    private static string BaseUrl => _baseUrl;

    [ClassInitialize]
    public static async Task StartApplication(TestContext _)
    {
        var configuredUrl = Environment.GetEnvironmentVariable("BIOTLAB_E2E_URL");
        if (!string.IsNullOrWhiteSpace(configuredUrl))
        {
            _baseUrl = configuredUrl.TrimEnd('/');
            await WaitUntilApplicationIsReady();
            return;
        }

        _webProjectDirectory = FindWebProjectDirectory();
        var projectFile = Path.Combine(_webProjectDirectory, "BiotLabWeb.csproj");

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            WorkingDirectory = _webProjectDirectory,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        startInfo.ArgumentList.Add("run");
        startInfo.ArgumentList.Add("--project");
        startInfo.ArgumentList.Add(projectFile);
        startInfo.ArgumentList.Add("--no-build");
        startInfo.ArgumentList.Add("--launch-profile");
        startInfo.ArgumentList.Add("https");

        _applicationProcess = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Não foi possível iniciar a aplicação BiotLab.");

        await WaitUntilApplicationIsReady();
    }

    [ClassCleanup]
    public static void StopApplication()
    {
        if (_applicationProcess is null || _applicationProcess.HasExited)
        {
            return;
        }

        _applicationProcess.Kill(entireProcessTree: true);
        _applicationProcess.WaitForExit(TimeSpan.FromSeconds(10));
        _applicationProcess.Dispose();
    }

    [TestInitialize]
    public void Initialize()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--window-size=1440,1000");
        options.AddArgument("--ignore-certificate-errors");
        options.AddArgument("--allow-insecure-localhost");
        options.AddArgument("--disable-search-engine-choice-screen");

        var driverDirectory = Path.GetDirectoryName(typeof(SeleniumSmokeTests).Assembly.Location)
            ?? AppContext.BaseDirectory;
        var driverService = ChromeDriverService.CreateDefaultService(driverDirectory);
        driverService.HideCommandPromptWindow = true;

        _driver = new ChromeDriver(driverService, options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
        {
            CaptureScreenshot($"{TestContext.TestName}-falha");
        }

        _driver?.Quit();
        _driver?.Dispose();
    }

    [TestMethod]
    public void LoginPage_LoadsRequiredControls()
    {
        Driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");

        Assert.AreEqual("account", Wait.Until(driver => driver.FindElement(By.TagName("form"))).GetAttribute("id"));
        Assert.IsTrue(Driver.FindElement(By.Id("Input_Email")).Displayed);
        Assert.IsTrue(Driver.FindElement(By.Id("Input_Password")).Displayed);
        Assert.IsTrue(Driver.FindElement(By.Id("login-submit")).Enabled);
        CaptureScreenshot("01-pagina-login");
    }

    [TestMethod]
    public void ProtectedModule_RedirectsAnonymousUserToLogin()
    {
        Driver.Navigate().GoToUrl($"{BaseUrl}/Instituicao");

        Wait.Until(driver => driver.Url.Contains("/Identity/Account/Login", StringComparison.OrdinalIgnoreCase));
        StringAssert.Contains(Driver.Url, "ReturnUrl=");
        CaptureScreenshot("02-redirecionamento-login");
    }

    [TestMethod]
    public void Administrator_CanLoginAndSeeDashboard()
    {
        var email = Environment.GetEnvironmentVariable("BIOTLAB_E2E_EMAIL");
        var password = Environment.GetEnvironmentVariable("BIOTLAB_E2E_PASSWORD");

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            Assert.Inconclusive(
                "Defina BIOTLAB_E2E_EMAIL e BIOTLAB_E2E_PASSWORD para executar o cenário autenticado.");
        }

        Driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");
        Wait.Until(driver => driver.FindElement(By.Id("Input_Email"))).SendKeys(email);
        Driver.FindElement(By.Id("Input_Password")).SendKeys(password);
        Driver.FindElement(By.Id("login-submit")).Click();

        Wait.Until(driver => !driver.Url.Contains("/Identity/Account/Login", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(Driver.PageSource.Contains("Dashboard BiotLab", StringComparison.Ordinal));
        CaptureScreenshot("03-dashboard-administrador");
    }

    private void CaptureScreenshot(string name)
    {
        if (_driver is not ITakesScreenshot screenshotDriver)
        {
            return;
        }

        var configuredDirectory = Environment.GetEnvironmentVariable("BIOTLAB_E2E_SCREENSHOTS");
        var outputDirectory = string.IsNullOrWhiteSpace(configuredDirectory)
            ? Path.Combine(TestContext.TestRunDirectory ?? AppContext.BaseDirectory, "SeleniumScreenshots")
            : configuredDirectory;

        Directory.CreateDirectory(outputDirectory);
        var safeName = string.Concat(name.Select(character =>
            Path.GetInvalidFileNameChars().Contains(character) ? '-' : character));
        screenshotDriver.GetScreenshot().SaveAsFile(
            Path.Combine(outputDirectory, $"{safeName}.png"));
    }

    private static async Task WaitUntilApplicationIsReady()
    {
        using var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
            AllowAutoRedirect = true
        };
        using var client = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(3)
        };

        var deadline = DateTime.UtcNow.AddSeconds(30);
        Exception? lastError = null;

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                using var response = await client.GetAsync($"{BaseUrl}/Identity/Account/Login");
                if (response.StatusCode is HttpStatusCode.OK)
                {
                    return;
                }
            }
            catch (Exception exception)
            {
                lastError = exception;
            }

            await Task.Delay(500);
        }

        throw new InvalidOperationException(
            $"A aplicação não ficou disponível em {BaseUrl}.", lastError);
    }

    private static string FindWebProjectDirectory()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "BiotLabWeb", "BiotLabWeb.csproj");
            if (File.Exists(candidate))
            {
                return Path.GetDirectoryName(candidate)!;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Não foi possível localizar BiotLabWeb/BiotLabWeb.csproj.");
    }

    private IWebDriver Driver =>
        _driver ?? throw new InvalidOperationException("WebDriver não inicializado.");

    private WebDriverWait Wait =>
        _wait ?? throw new InvalidOperationException("Espera do WebDriver não inicializada.");
}
