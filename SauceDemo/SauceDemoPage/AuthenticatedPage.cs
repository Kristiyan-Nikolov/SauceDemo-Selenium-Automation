using SeleniumExtras.WaitHelpers;

using OpenQA.Selenium;


namespace SauceDemo.SauceDemoPage
{
    public abstract class AuthenticatedPage : BasePage
    {
        public AuthenticatedPage(IWebDriver driver) : base(driver) { }
        private By AllMenuBtnLocator => By.Id("react-burger-menu-btn");
        private By LogoutBtnLocator => By.Id("logout_sidebar_link");
        private By AboutBtnLocator => By.Id("about_sidebar_link");

        // Footer
        private By TwitterBtnLocator => By.CssSelector("[data-test='social-twitter']");
        private By FacebookBtnLocator => By.CssSelector("[data-test='social-facebook']");
        private By LinkedInBtnLocator => By.CssSelector("[data-test='social-linkedin']");
        private By CartIconLocator => By.CssSelector("[data-test='shopping-cart-badge']");
        private By FooterLocator => By.CssSelector("[data-test='footer-copy']");
        private By BurgerMenuLocator => By.ClassName("bm-menu-wrap");
        private By CloseMenuBtnLocator => By.Id("react-burger-cross-btn");

        public string GetTitle()
        {
            var title = WaitAndFind(By.XPath("//*[@id=\"header_container\"]/div[2]/span"));
            return title.Text;
        }
        public void OpenMenu()
        {
            ClickWhenClickable(AllMenuBtnLocator);
        }
        public LoginPage Logout()
        {
         
            ClickWhenClickable(LogoutBtnLocator);
            return new LoginPage(driver);
        }
        public void About()
        {
            ClickWhenClickable(AboutBtnLocator);
        }
        public void Twitter()
        {
            ScrollToElement(FooterLocator);
            ClickWhenClickable(TwitterBtnLocator);
        }
        public void Facebook() 
        {
            ScrollToElement(FooterLocator);
            ClickWhenClickable(FacebookBtnLocator);
        }
        public void Linkedin()
        {
            ScrollToElement(FooterLocator);
            ClickWhenClickable(LinkedInBtnLocator);
        }
        public CartPage OpenCart()
        {
            ClickWhenClickable(CartIconLocator);
            return new CartPage(driver);
        }
        // Get cart count and return 0 if cart is empty.
        public int GetCartCount()
        { 
            try
            {
                var element = driver.FindElements(CartIconLocator);
                if (element.Count == 0)
                    return 0; // badge exists but empty

                return int.Parse(element[0].Text);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Cart badge not found — cart is empty.");
                return 0;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Cart badge not found — cart is empty.");
                return 0;
            }
        }
        public bool IsBurgerMenuOpened()
        {
            var burgerMenu = WaitAndFindAllVisible(BurgerMenuLocator);
            return burgerMenu.Count > 0;
        }
        public bool IsBurgerMenuClosed()
        {
            try
            {
                // Wait until the menu is invisible
                return wait.Until(ExpectedConditions.InvisibilityOfElementLocated(BurgerMenuLocator));
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        public void CloseMenu()
        {
            ClickWhenClickable(CloseMenuBtnLocator);
        }



    }
    
}
