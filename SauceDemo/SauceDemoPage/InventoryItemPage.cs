using OpenQA.Selenium;


namespace SauceDemo.SauceDemoPage
{
    public class InventoryItemPage : AuthenticatedPage
    {
        public InventoryItemPage(IWebDriver driver) : base(driver) { }
        private By BackToProductsLocator => By.Id("back-to-products");
        private By TitleItemNameLocator => By.CssSelector("[data-test='inventory-item-name']");
        private By DescriptionItemLocator => By.CssSelector("[data-test='inventory-item-desc']");
        private By PriceItemLocator => By.CssSelector("[data-test='inventory-item-price']");
        private By AddToCartLocator => By.Id("add-to-cart");
        protected override By StableLocator => By.Id("back-to-products");
        private By RemoveItemBtnLocator => By.Id("remove");
        public string GetItemName()
        {
            var itemName = WaitAndFind(TitleItemNameLocator);
            return itemName.Text;
        }
        public string GetDescription()
        {
            var itemDescription = WaitAndFind(DescriptionItemLocator);
            return itemDescription.Text;
        }
        public double GetPrice()
        {
            var price = WaitAndFind(PriceItemLocator);
            return double.Parse(price.Text.Trim('$'));
        }
        public void AddToCart()
        {
            ClickWhenClickable(AddToCartLocator);
        }
        public InventoryPage ReturnToInventory()
        {
            ClickWhenClickable(BackToProductsLocator);
            return new InventoryPage(driver);
        }
        public void RemoveItem()
        {
            ClickWhenClickable(RemoveItemBtnLocator);
        }
        
    }
}
