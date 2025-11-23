using OpenQA.Selenium;


namespace SauceDemo.SauceDemoPage
{
    public class CheckoutPage_StepTwo : AuthenticatedPage
    {
        public CheckoutPage_StepTwo(IWebDriver driver) : base(driver) { }
        public override string pageURL => "https://www.saucedemo.com/checkout-step-two.html";
        private By CheckoutItemNamesLocator => By.CssSelector("[data-test='inventory-item-name']");
        private By CheckoutItemPricesLocator => By.CssSelector("[data-test='inventory-item-price']");
        private By TotalPriceWithoutTaxLocator => By.CssSelector("[data-test='subtotal-label']");
        private By TaxLocator => By.CssSelector("[data-test='tax-label']");
        private By TotalPriceWithTaxLocator => By.CssSelector("[data-test='total-label']");
        private By CancelBtnLocator => By.CssSelector("[data-test='cancel']");
        private By FinishBtnLocator => By.CssSelector("[data-test='finish']");
        protected override By StableLocator => By.Id("checkout_summary_container");
        public List<double> PricesOfItems()
        {
            
            var priceList = WaitAndFindAllVisible(CheckoutItemPricesLocator);
            return priceList.Select(x => double.Parse(x.Text.Replace("$",""))).ToList();
        }
        public List<string> GetItemNames()
        {
            
            var nameList = WaitAndFindAllVisible(CheckoutItemNamesLocator);
            return nameList.Select(x => x.Text).ToList();
        }
        public double GetPriceWithoutTax()
        {
            var priceWithoutTax = WaitAndFind(TotalPriceWithoutTaxLocator);
            return double.Parse(priceWithoutTax.Text.Replace("Item total: $",""));
        }
        public double GetTaxTotal()
        {
            var tax = WaitAndFind(TaxLocator);
            return double.Parse(tax.Text.Replace("Tax: $",""));
        }
        public double GetTotalPriceWithTax()
        {
            var totalPriceWithTax = WaitAndFind(TotalPriceWithTaxLocator);
            return double.Parse(totalPriceWithTax.Text.Replace("Total: $",""));
        }
        public double CalculateTax(double sum)
        {
            var taxCalc = sum * 0.08;
            taxCalc = Math.Round(taxCalc, 2);
            return taxCalc;
        }
        public InventoryPage ReturnToInventory()
        {
            ScrollToElement(CancelBtnLocator);
            ClickWhenClickable(CancelBtnLocator);
            return new InventoryPage(driver);
        }
        public CheckoutPage_Complete CheckoutCompleteStep()
        {
            ScrollToElement(FinishBtnLocator);
            ClickWhenClickable(FinishBtnLocator);
            return new CheckoutPage_Complete(driver);
        }
    }
    
}
