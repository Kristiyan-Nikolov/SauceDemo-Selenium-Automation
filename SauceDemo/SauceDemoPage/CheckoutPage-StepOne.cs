using OpenQA.Selenium;


namespace SauceDemo.SauceDemoPage
{
    public class CheckoutPage_StepOne : AuthenticatedPage
    {
        public CheckoutPage_StepOne(IWebDriver driver) : base(driver) { }
        private By CancelBtnLocator => By.Id("cancel");
        private By ContinueBtnLocator => By.Id("continue");
        private By FirstNameLocator => By.Id("first-name");
        private By LastNameLocator => By.Id("last-name");
        private By ZipCodeLocator => By.Id("postal-code");
        private By ErrorLocator => By.CssSelector("[data-test='error']");
        public override string pageURL => "https://www.saucedemo.com/checkout-step-one.html";
        protected override By StableLocator => By.Id("checkout_info_container");
        public void FillCheckoutInformation(string firstName, string lastName, string zipCode)
        {
            WaitAndFind(FirstNameLocator).SendKeys(firstName);
            WaitAndFind(LastNameLocator).SendKeys(lastName);
            WaitAndFind(ZipCodeLocator).SendKeys(zipCode);
            
        }
        public void SubmitFormWhenEmpty()
        {
            ScrollToElement(ContinueBtnLocator);
            ClickWhenClickable(ContinueBtnLocator);
        }
        public CheckoutPage_StepTwo SubmitFormAndProceedStepTwo()
        {
            ScrollToElement(ContinueBtnLocator);
            ClickWhenClickable(ContinueBtnLocator);
            return new CheckoutPage_StepTwo(driver);

        }
        public string GetErrorMsg()
        {
            var errorMsg = WaitAndFind(ErrorLocator);
            return errorMsg.Text;
        }
        public CartPage ReturnToCart()
        {
            ClickWhenClickable(CancelBtnLocator);
            return new CartPage(driver);
        }
    }
    

}
