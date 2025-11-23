using SauceDemo.Data;
using SauceDemo.SauceDemoPage;
using System.Diagnostics;


namespace SauceDemo.SauceDemoTests
{
    
    public class LoginTests : BaseTests
    {
        [Category("Authentication")]
        [Category("Smoke")]
        [Test]
        public void Login_WithValidCredentials_ShouldOpenInventory()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();
            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);

            Assert.That(inventoryPage.GetTitle(), Is.EqualTo(PageTitles.InventoryTitle));
        }
        [Category("Authentication")]
        [Test]
        public void Login_WithInvalidCredentials_ShouldShowError()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();
            var inventoryPage = loginPage.Login(UserData.ImaginaryUser, UserData.Password);

            var error = loginPage.GetErrorMessage();
            Assert.That(error, Is.EqualTo(LoginErrors.ErrorNoSuchUser));
            

        }
        [Category("Authentication")]
        [Test]
        public void Login_WithLockedCredentials_ShouldShowLockedMessage()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();
            var inventoryPage = loginPage.Login(UserData.LockedUser, UserData.Password);

            var error = loginPage.GetErrorMessage();
            Assert.That(error, Is.EqualTo(LoginErrors.ErrorUserLockedOut));
        }
        [Category("Authentication")]
        [Test]
        public void Login_WithPerformanceGlitchedCredentials_ShouldLoadInventoryEventually()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var invetoryPage = loginPage.Login(UserData.GlitchUser, UserData.Password);
            Assert.That(invetoryPage.WaitForStablePage(), Is.True);
            stopWatch.Stop();
            Assert.That(stopWatch.ElapsedMilliseconds, Is.GreaterThan(2000), "Performance glitched user logged in too fast (greaterThan 2 seconds).");
            

        }
        [Category("Authentication")]
        [Test]
        public void Login_WithProblemCredentials_ShouldOpenInventory()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.ProblemUser, UserData.Password);

            Assert.That(inventoryPage.IsOpen(), Is.True, "Inventory page did not open.");
            Assert.That(inventoryPage.GetTitle(), Is.EqualTo(PageTitles.InventoryTitle));
        }
        [Category("Authentication")]
        [Test]
        public void Login_WithEmptyFields_ShouldShowRequiredFieldsErrors()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.EmptyUser, UserData.EmptyPassword);
            Assert.That(inventoryPage.IsOpen(), Is.False);
            Assert.That(loginPage.GetErrorMessage(), Is.EqualTo(LoginErrors.ErrorUsernameRequired));
        }
        [Category("Authentication")]
        [Test]
        public void Login_WithEmptyUsername_ShouldShowError()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.EmptyUser, UserData.Password);
            Assert.That(inventoryPage.IsOpen(), Is.False);
            Assert.That(loginPage.GetErrorMessage(), Is.EqualTo(LoginErrors.ErrorUsernameRequired));
        }
        [Category("Authentication")]
        [Test]
        public void Login_WithEmptyPassword_ShouldShowError()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.EmptyPassword);
            Assert.That(inventoryPage.IsOpen(), Is.False);
            Assert.That(loginPage.GetErrorMessage(), Is.EqualTo(LoginErrors.ErrorPasswordRequired));
        }
        [Category("Authentication")]
        [Test]
        public void Login_WithInvalidCredentials_CloseErrorMessage_ShouldHideField()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            loginPage.Login(UserData.StandardUser, UserData.EmptyPassword);
            
            Assert.That(loginPage.GetErrorMessage(), Is.EqualTo(LoginErrors.ErrorPasswordRequired));

            loginPage.CloseErrorMessage();
            var messageRemoved = loginPage.WaitForErrorGone();
            Assert.That(messageRemoved, Is.True);
        }
    }
}
