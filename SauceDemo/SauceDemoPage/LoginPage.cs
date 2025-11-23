using OpenQA.Selenium;
using System.Linq.Expressions;


namespace SauceDemo.SauceDemoPage
{
    
    public class LoginPage : BasePage
    {
        
        public LoginPage(IWebDriver driver) : base(driver) { }
        public override string pageURL => "https://www.saucedemo.com/";
        private By UsernameInputLocator => By.Id("user-name");
        private By PasswordInputLocator => By.Id("password");
        private By LoginBtnLocator => By.Id("login-button");
        private By ErrorFieldLocator => By.XPath("//*[@id=\"login_button_container\"]/div/form/div[3]/h3");
        private By ErrorFieldBtnLocator => By.CssSelector("[data-test='error-button']");
        protected override By StableLocator => By.Id("login-button");
       
        public InventoryPage Login(string username, string password)
        {
            // Wait for username to be visible with the helper method created from BasePage
            var usernameInput = WaitAndFind(UsernameInputLocator);
            usernameInput.SendKeys(username);

            var passwordInput = WaitAndFind(PasswordInputLocator);
            passwordInput.SendKeys(password);

            // Locate the login button
            ClickWhenClickable(LoginBtnLocator);

            return new InventoryPage(driver);
        }
       
        public string GetErrorMessage()
        {
            // Gets the h3 error msg 
            var error = WaitAndFind(ErrorFieldLocator);
            return error.Text;
        }
        public void CloseErrorMessage()
        {
            ScrollToElement(ErrorFieldBtnLocator);
            
            ClickWhenClickable(ErrorFieldBtnLocator);
        }
        public bool WaitForErrorGone()
        {
            try
            {
                return wait.Until(d =>
                {
                    var elements = d.FindElements(ErrorFieldLocator);
                    return elements.Count == 0 || !elements[0].Displayed;
                });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }

            
        }
        
        
    }
}
