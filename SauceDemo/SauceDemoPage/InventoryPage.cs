using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace SauceDemo.SauceDemoPage
{
    public class InventoryPage : AuthenticatedPage
    {
        public InventoryPage(IWebDriver driver) : base(driver) { }
        public override string pageURL => "https://www.saucedemo.com/inventory.html";
        private By ProductSortLocator => By.CssSelector("[data-test='product-sort-container']");
        private By GetItemNamesLocator => By.CssSelector("[data-test='inventory-item-name']");
        private By GetPricesLocator => By.CssSelector("[data-test='inventory-item-price']");
        private By GetDescriptionLocator => By.XPath("//div[@class='inventory_item_desc']");
        protected override By StableLocator => By.Id("inventory_container");

        // Dynamically take element by itemName
        public void AddItem(string itemName)
        {
            var findSpecificBuyButton = By.XPath($"//div[normalize-space(text())='{itemName}']/ancestor::div[@class='inventory_item']//button");
            ClickWhenClickable(findSpecificBuyButton);
        }
        public void RemoveItem(string itemName)
        {
            var removeBtn = By.XPath($"//div[normalize-space(text())='{itemName}']/ancestor::div[@class='inventory_item']//button[contains(text(),'Remove')]");
            ClickWhenClickable(removeBtn);
        }
        public void SortBy(string typeOfSort)
        {
            var dropdown = new SelectElement(WaitAndFind(ProductSortLocator));
            dropdown.SelectByText(typeOfSort);
        }
        public List<string> GetItemNames()
        {
            
            var listNames = WaitAndFindAllVisible(GetItemNamesLocator); // returns Ilist<IwebElement>
            return listNames.Select(n => n.Text).ToList();
        }
        public List<string> GetPrices()
        {
           
            var pricesOfItems = WaitAndFindAllVisible(GetPricesLocator);
            return pricesOfItems.Select(n => n.Text).ToList();
        }
        public InventoryItemPage GoToItemPage(string itemName)
        {
            var itemLocator = By.XPath($"//div[normalize-space(text())='{itemName}']");
            ClickWhenClickable(itemLocator);
            return new InventoryItemPage(driver);
        }
        
        // Dynamic button locator needed as well...
        private By ItemButtonLocator(string itemName)
        {
            return By.XPath($"//div[normalize-space(text())='{itemName}']/ancestor::div[@class='inventory_item']//button");
        }
        public bool IsAddToCartButtonVisible(string itemName)
        {
            var button = WaitAndFind(ItemButtonLocator(itemName));
            return button.Text == "Add to cart";
        }
        public List<string> GetAllDescriptions()
        {
            var description = WaitAndFindAllVisible(GetDescriptionLocator);
            return description.Select(x => x.Text).ToList();
        }
        public List<double> GetAllPrices()
        {
            var prices = WaitAndFindAllVisible(GetPricesLocator);
            return prices.Select(x => double.Parse(x.Text.Replace("$", ""))).ToList();
        }

    }
    
}


