using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace SauceDemo.SauceDemoPage
{
    public abstract class BasePage
    {
        protected readonly IWebDriver driver;
        protected WebDriverWait wait;
        protected abstract By StableLocator {  get; }
        public BasePage(IWebDriver driver)
        {
            this.driver = driver; // Instance of driver
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        protected IWebElement WaitAndFind(By locator)
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
            
        }
        protected void ClickWhenClickable(By locator)
        {
            var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            element.Click();
        }
        // Use for dynamic page instead of IsOpen
        public bool WaitForStablePage()
        {
            try
            {
                var element = WaitAndFind(StableLocator);
                return element.Displayed;
            }
            catch
            {
                return false;
            }

        }
        
        public virtual string pageURL { get; }
    
        public void OpenPage()
        {
            driver.Navigate().GoToUrl(pageURL);
        }
        public bool IsOpen()
        {
            return driver.Url == this.pageURL;
        }

        protected void ScrollToElement(By locator)
        {
            var element = WaitAndFind(locator);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element); // Need to learn more about these JSExecutors
        }

        public void SwitchToNewTabAndWaitForUrl(string expectedUrl = null)
        {
            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs.Last());
            if (expectedUrl == null)
            {
                wait.Until(d => d.Url != "about:blank");

            }
            else 
            {
                wait.Until(d => d.Url == expectedUrl);
            }
        }
        protected IReadOnlyCollection<IWebElement> WaitAndFindAllVisible(By locator)
        {
            try
            {
                var list = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
                return list;
            }
            catch (WebDriverTimeoutException) 
            {
                return Array.Empty<IWebElement>();
            }
        }


    }
}
