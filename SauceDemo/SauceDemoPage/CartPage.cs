using OpenQA.Selenium;


namespace SauceDemo.SauceDemoPage
{
    public class CartPage : AuthenticatedPage
    {
        public CartPage(IWebDriver driver) : base(driver) { }
        private By CartItemNameLocator => By.CssSelector("[data-test='inventory-item-name']");
        private By InventoryReturnBtnLocator => By.Id("continue-shopping");
        private By CheckoutBtnLocator => By.Id("checkout");
        public override string pageURL => "https://www.saucedemo.com/cart.html";
        protected override By StableLocator => By.Id("cart_contents_container");
        public void RemoveItem(string cartName)
        {
            var removeBtn = By.XPath($"//div[normalize-space(text())='{cartName}']/ancestor::div[@class='cart_item']//button[contains(text(),'Remove')]");
            ClickWhenClickable(removeBtn);
        }
        public List<string> GetCartNames()
        {
            
            var cartNames = WaitAndFindAllVisible(CartItemNameLocator);
            return cartNames.Select(x => x.Text).ToList();
        }
        // Outdated removeAll method. Use removeItem in a loop with the inventoryData array
        public InventoryPage ReturnToInventory()
        {
            ClickWhenClickable(InventoryReturnBtnLocator);
            var inventoryPage = new InventoryPage(driver);
            
            wait.Until(d => inventoryPage.IsOpen());
            return inventoryPage;
        }
        public CheckoutPage_StepOne GoToCheckout()
        {
            ClickWhenClickable(CheckoutBtnLocator);
            return new CheckoutPage_StepOne(driver);
        }
        public string GetItemName(string itemName)
        {
            var item = By.XPath($"//div[@class='inventory_item_name' and text()='{itemName}']");
            var element = WaitAndFind(item);
            return element.Text;
        }
    }
}
