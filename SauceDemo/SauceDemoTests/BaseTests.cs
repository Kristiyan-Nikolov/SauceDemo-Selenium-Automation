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

            // Remove comment for headless mode
            //options.AddArgument("--headless");

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