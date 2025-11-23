using OpenQA.Selenium;


namespace SauceDemo.SauceDemoPage
{
    public class CheckoutPage_Complete : AuthenticatedPage
    {
        public CheckoutPage_Complete(IWebDriver driver) : base(driver) { }
        private By BackToHomeLocator => By.Id("back-to-products");
        private By OrderConfirmationLocator => By.CssSelector("[data-test='complete-header']");
        protected override By StableLocator => By.Id("checkout_complete_container");

        public InventoryPage BackToInventory()
        {
            ScrollToElement(BackToHomeLocator);
            ClickWhenClickable(BackToHomeLocator);
            return new InventoryPage(driver);
        }
        public bool ConfirmationMessage()
        {
            var msg = WaitAndFind(OrderConfirmationLocator);
            if (msg.Text == "Thank you for your order!")
            {
                return true;
            }
            else { return false; }
        }
    }
}
