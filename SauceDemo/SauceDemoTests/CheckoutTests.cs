using SauceDemo.SauceDemoPage;
using SauceDemo.Data;

namespace SauceDemo.SauceDemoTests
{
    public class CheckoutTests : BaseTests
    {
        // Darkness, asserting me. All that I see, absolute horror. I cannot live, I cannot die.
        [Category("Smoke")]
        [Category("Checkout")]
        [Test]
        public void FillCheckoutInformation_WithValidData_ShouldProceedToCheckoutOverview()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            Assert.Multiple(() =>
            {
                Assert.That(inventoryPage.GetTitle(), Is.EqualTo(PageTitles.InventoryTitle));
                Assert.That(inventoryPage.IsOpen(), Is.True, "Failed to retrieve inventory URL");
            });
            
            inventoryPage.AddItem(InventoryData.Backpack);
            var cartPage = inventoryPage.OpenCart();
            Assert.Multiple(() =>
            {
                Assert.That(cartPage.GetTitle(), Is.EqualTo(PageTitles.CartTitle));
                Assert.That(cartPage.IsOpen(), Is.True, "Failed to retrieve cart URL");
            });
            
            var checkoutPage_partOne = cartPage.GoToCheckout();
            Assert.Multiple(() =>
            {
                Assert.That(checkoutPage_partOne.GetTitle(), Is.EqualTo(PageTitles.CheckoutOne));
                Assert.That(checkoutPage_partOne.IsOpen(), Is.True, "Failed to retrieve checkout part one URL");
            });
            
            checkoutPage_partOne.FillCheckoutInformation(CheckoutData.FirstName, CheckoutData.LastName, CheckoutData.PostalCode);
            var checkoutPage_partTwo = checkoutPage_partOne.SubmitFormAndProceedStepTwo();
            Assert.Multiple(() =>
            {
                Assert.That(checkoutPage_partTwo.GetTitle(), Is.EqualTo(PageTitles.CheckoutTwo));
                Assert.That(checkoutPage_partTwo.IsOpen(), Is.True, "Failed to retrieve checkout part two URL");
            });
            

        }
        
        [Test]
        [Category("Checkout")]
        public void Checkout_FirstNameMissing_ShouldReturnsErrorMsg()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.AddItem(InventoryData.Backpack);
            var cartPage = inventoryPage.OpenCart();
            var checkoutPage_partOne = cartPage.GoToCheckout();
            checkoutPage_partOne.FillCheckoutInformation(CheckoutData.EmptyFirstName, CheckoutData.LastName, CheckoutData.PostalCode);
            checkoutPage_partOne.SubmitFormWhenEmpty();
            var errorMsgFirstName = checkoutPage_partOne.GetErrorMsg();
            Assert.That(errorMsgFirstName, Is.EqualTo(CheckoutOneErrors.ErrorFirstNameRequired));
        }
        [Category("Checkout")]
        [Test]
        public void Checkout_LastNameMissing_ShouldReturnsErrorMsg()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.AddItem(InventoryData.Backpack);
            var cartPage = inventoryPage.OpenCart();
            var checkoutPage_partOne = cartPage.GoToCheckout();
            checkoutPage_partOne.FillCheckoutInformation(CheckoutData.FirstName, CheckoutData.EmptyLastName, CheckoutData.PostalCode);
            checkoutPage_partOne.SubmitFormWhenEmpty();
            var errorMsgFirstName = checkoutPage_partOne.GetErrorMsg();
            Assert.That(errorMsgFirstName, Is.EqualTo(CheckoutOneErrors.ErrorLastNameRequired));
        }
        [Category("Checkout")]
        [Test]
        public void Checkout_PostalCodeMissing_ShouldReturnsErrorMsg()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.AddItem(InventoryData.Backpack);
            var cartPage = inventoryPage.OpenCart();
            var checkoutPage_partOne = cartPage.GoToCheckout();
            checkoutPage_partOne.FillCheckoutInformation(CheckoutData.FirstName, CheckoutData.LastName, CheckoutData.EmptyPostalCode);
            checkoutPage_partOne.SubmitFormWhenEmpty();
            var errorMsgFirstName = checkoutPage_partOne.GetErrorMsg();
            Assert.That(errorMsgFirstName, Is.EqualTo(CheckoutOneErrors.ErrorPostalCodeRequired));
        }
        [Category("Checkout")]
        [Test]
        public void Checkout_CancelButton_ShouldReturnToCart()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.AddItem(InventoryData.Backpack);
            var cartPage = inventoryPage.OpenCart();
            var checkoutPage_partOne = cartPage.GoToCheckout();
            cartPage = checkoutPage_partOne.ReturnToCart();
            Assert.That(cartPage.IsOpen(), Is.True);
        }
        [Category("Checkout")]
        [Test]
        public void CheckoutOverview_TotalPrice_ShouldMatchCalculatedSubtotalAndTax()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);

            foreach (var item in InventoryData.ItemNames)
                inventoryPage.AddItem(item);

            var cartPage = inventoryPage.OpenCart();
            var checkoutPage_partOne = cartPage.GoToCheckout();
            checkoutPage_partOne.FillCheckoutInformation(CheckoutData.FirstName, CheckoutData.LastName, CheckoutData.PostalCode);
            var checkoutPage_partTwo = checkoutPage_partOne.SubmitFormAndProceedStepTwo();

            // Get total price without tax
            var priceList = checkoutPage_partTwo.PricesOfItems();       
            var actualPriceWithoutTax = priceList.Sum();                
            var expectedPriceWithoutTax = checkoutPage_partTwo.GetPriceWithoutTax();      
            
            // Get total tax
            var actualTax = checkoutPage_partTwo.CalculateTax(actualPriceWithoutTax);
            var expectedTax = checkoutPage_partTwo.GetTaxTotal();
            
            // Get total price with tax
            var actualTotal = actualTax + actualPriceWithoutTax;
            var expectedTotal = checkoutPage_partTwo.GetTotalPriceWithTax();
           
            Assert.Multiple(() =>
            {
                Assert.That(expectedPriceWithoutTax, Is.EqualTo(actualPriceWithoutTax));
                Assert.That(actualTax, Is.EqualTo(expectedTax));
                Assert.That(actualTotal, Is.EqualTo(expectedTotal));
            });
        }
        [Category("Checkout")]
        [Test]
        public void CartItems_ShouldMatchCheckoutOverviewItems()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            foreach (var item in InventoryData.ItemNames)
                inventoryPage.AddItem(item);

            var cartPage = inventoryPage.OpenCart();

            var expectedListNames = cartPage.GetCartNames();

            var checkoutPage_StepOne = cartPage.GoToCheckout();
            checkoutPage_StepOne.FillCheckoutInformation(CheckoutData.FirstName, CheckoutData.LastName, CheckoutData.PostalCode);

            var checkoutPage_StepTwo = checkoutPage_StepOne.SubmitFormAndProceedStepTwo();
            var actualList = checkoutPage_StepTwo.GetItemNames();

            Assert.That(expectedListNames, Is.EquivalentTo(actualList));

        }
        [Category("Smoke")]
        [Category("Checkout")]
        [Test]
        public void CompleteCheckout_WithValidData_ShouldShowSuccessMessage()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            foreach (var item in InventoryData.ItemNames)
                inventoryPage.AddItem(item);

            var cartPage = inventoryPage.OpenCart();
            var checkoutPage_StepOne = cartPage.GoToCheckout();
            checkoutPage_StepOne.FillCheckoutInformation(CheckoutData.FirstName, CheckoutData.LastName, CheckoutData.PostalCode);
            var checkoutPage_StepTwo = checkoutPage_StepOne.SubmitFormAndProceedStepTwo();
            var checkoutPage_Complete = checkoutPage_StepTwo.CheckoutCompleteStep();

            var successfullMsg = checkoutPage_Complete.ConfirmationMessage();
            Assert.That(successfullMsg, Is.True);
        }
        [Category("Checkout")]
        [Test]
        public void CompleteCheckout_AfterPurchase_LogoutShouldReturnToLogin()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            foreach (var item in InventoryData.ItemNames)
                inventoryPage.AddItem(item);

            var cartPage = inventoryPage.OpenCart();
            var checkoutPage_StepOne = cartPage.GoToCheckout();
            checkoutPage_StepOne.FillCheckoutInformation(CheckoutData.FirstName, CheckoutData.LastName, CheckoutData.PostalCode);
            var checkoutPage_StepTwo = checkoutPage_StepOne.SubmitFormAndProceedStepTwo();
            var checkoutPage_Complete = checkoutPage_StepTwo.CheckoutCompleteStep();

            var successfullMsg = checkoutPage_Complete.ConfirmationMessage();
            Assert.That(successfullMsg, Is.True);

            inventoryPage = checkoutPage_Complete.BackToInventory();
            Assert.That(inventoryPage.GetTitle(), Is.EqualTo(PageTitles.InventoryTitle));

            inventoryPage.OpenMenu();
            loginPage = inventoryPage.Logout();
            Assert.That(loginPage.IsOpen(), Is.True);
        }


    }
}
