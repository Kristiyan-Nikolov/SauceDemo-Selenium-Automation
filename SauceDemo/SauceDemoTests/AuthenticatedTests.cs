using SauceDemo.Data;
using SauceDemo.SauceDemoPage;


namespace SauceDemo.SauceDemoTests
{
    public class AuthenticatedTests : BaseTests
    {
        [Category("Authentication")]
        [Test]
        public void Logout_ShouldReturnToLoginPage()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();


            var authenticatedPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            Assert.That(authenticatedPage.GetTitle(), Is.EqualTo(PageTitles.InventoryTitle));
            authenticatedPage.OpenMenu();
            loginPage = authenticatedPage.Logout();
            
            Assert.That(loginPage.IsOpen(), Is.True);
        }
        [Category("Navigation")]
        [Test]
        public void AboutLink_ShouldNavigateToAboutPage()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();


            var authenticatedPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            Assert.That(authenticatedPage.GetTitle(), Is.EqualTo(PageTitles.InventoryTitle));
            authenticatedPage.OpenMenu();
            authenticatedPage.About();

            Assert.That(driver.Url, Is.EqualTo(ExternalLinks.SauceLabs));
        }
        [Category("Navigation")]
        [Test]
        public void FacebookLink_ShouldNavigateToFacebookPage()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();


            var authenticatedPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            Assert.That(authenticatedPage.GetTitle(), Is.EqualTo(PageTitles.InventoryTitle));
            
            authenticatedPage.Facebook();
            authenticatedPage.SwitchToNewTabAndWaitForUrl();

            Assert.That(driver.Url, Is.EqualTo(ExternalLinks.Facebook));
            Console.WriteLine("Facebook link successfully reached");
        }
        [Category("Navigation")]
        [Test]
        public void TwitterLink_ShouldNavigateToTwitterPage()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();


            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            Assert.That(inventoryPage.GetTitle(), Is.EqualTo(PageTitles.InventoryTitle));

            inventoryPage.Twitter();
            inventoryPage.SwitchToNewTabAndWaitForUrl();

            Assert.That(driver.Url, Is.EqualTo(ExternalLinks.Xcom));
            Console.WriteLine("Twitter link successfully reached");
        }
        [Category("Navigation")]
        [Test]
        public void LinkedInLink_ShouldNavigateToLinkedInPage()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            Assert.That(inventoryPage.GetTitle(), Is.EqualTo(PageTitles.InventoryTitle));

            inventoryPage.Linkedin();
            inventoryPage.SwitchToNewTabAndWaitForUrl();

            Assert.That(driver.Url, Is.EqualTo(ExternalLinks.Linkedin));
            Console.WriteLine("Linkedin link successfully reached");
        }
        [Category("Navigation")]
        [Test]
        public void Menu_ShouldOpen_WhenClicked()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            Assert.That(inventoryPage.GetTitle(), Is.EqualTo(PageTitles.InventoryTitle));

            inventoryPage.OpenMenu();
            Assert.That(inventoryPage.IsBurgerMenuOpened(), Is.True);
        }
        [Category("Navigation")]
        [Test]
        public void Menu_ShouldClose_WhenCloseClicked()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            Assert.That(inventoryPage.GetTitle(), Is.EqualTo(PageTitles.InventoryTitle));

            inventoryPage.OpenMenu();
            Assert.That(inventoryPage.IsBurgerMenuOpened(), Is.True);

            inventoryPage.CloseMenu();
            Assert.That(inventoryPage.IsBurgerMenuClosed(), Is.True);
        }
    }
}
