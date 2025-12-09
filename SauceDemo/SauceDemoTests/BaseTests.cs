using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;
using System.Linq;
using SauceDemo.Data;
using OpenQA.Selenium.Support;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Support.Extensions;

namespace SauceDemo.SauceDemoTests
{
    public class BaseTests
    {
        protected IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new FirefoxOptions();

            options.SetPreference("signon.rememberSignons", false);
            options.SetPreference("signon.autofillForms", false);
            options.SetPreference("signon.management.page.breach-alert", false);
            options.SetPreference("identity.fxaccounts.enabled", false);
            options.SetPreference("services.sync.engine.passwords", false);
            options.SetPreference("browser.shell.checkDefaultBrowser", false);
            options.SetPreference("browser.startup.homepage_override.mstone", "ignore");

            options.AddArgument("-no-remote");
            options.AddArgument("-private");
            options.AddArgument("no-sandbox");
            options.AddArgument("disable-dev-shm-usage");
            options.AddArgument("disable-gpu");
            options.AddArgument("window-size=1920x1080");

            // Remove comment for headless mode
            options.AddArgument("--headless");

            // Create geckodriver service with logging
            var service = FirefoxDriverService.CreateDefaultService();
            service.LogPath = Path.Combine(Directory.GetCurrentDirectory(), "geckodriver.log");
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            // Start Firefox
            driver = new FirefoxDriver(service, options, TimeSpan.FromSeconds(60));
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            // Modify project root 
            var projectRoot = @"C:\Users\User\Desktop\SauceDemo-Selenium-Automation\SauceDemo";
            var screenshotsDir = Path.Combine(projectRoot, "Screenshots");
            var logsDir = Path.Combine(projectRoot, "Logs");

            // Ensure folders exist
            Directory.CreateDirectory(screenshotsDir);
            Directory.CreateDirectory(logsDir);

            // Check if the test failed
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;
            if (outcome == TestStatus.Failed && driver != null)
            {
                try
                {

                    var invalid = Path.GetInvalidFileNameChars();
                    var rawName = TestContext.CurrentContext.Test.Name ?? "UnknownTest";
                    var safeName = string.Join("_", rawName.Split(invalid, StringSplitOptions.RemoveEmptyEntries));


                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    var screenshotPath = Path.Combine(screenshotsDir, $"{safeName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    screenshot.SaveAsFile(screenshotPath);


                    TestContext.AddTestAttachment(screenshotPath, "Screenshot on failure");


                    var logs = driver.Manage().Logs.GetLog(LogType.Browser);
                    if (logs != null && logs.Count > 0)
                    {
                        var logPath = Path.Combine(logsDir, $"{safeName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                        File.WriteAllLines(logPath, logs.Select(l => $"{l.Timestamp} [{l.Level}] {l.Message}"));
                        TestContext.AddTestAttachment(logPath, "Browser console logs on failure");
                    }
                }
                catch (Exception ex)
                {
                    // report the teardown error so you can see why saving failed
                    TestContext.WriteLine($"TearDown: failed to capture artifacts: {ex.GetType().Name}: {ex.Message}");
                }
            }

            // Dispose the driver safely
            if (driver != null)
            {
                try { driver.Quit(); }
                catch { }
                finally
                {
                    driver.Dispose();
                    driver = null;
                }
            }
        }
    }
}