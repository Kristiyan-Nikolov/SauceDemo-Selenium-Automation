using SauceDemo.SauceDemoPage;
using SauceDemo.Data;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace SauceDemo.SauceDemoTests
{
    public class InventoryItemTests : BaseTests
    {
        [Category("ItemDetails")]
        [Test]
        public void InventoryItemName_ShouldMatchItemDetailName()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            var inventoryItemList = inventoryPage.GetItemNames();
            List<string> itemList = new List<string>();
            foreach (var item in InventoryData.ItemNames)
            {
                var inventoryItemPage = inventoryPage.GoToItemPage(item);
                var getName = inventoryItemPage.GetItemName();
                itemList.Add(getName);
                inventoryPage = inventoryItemPage.ReturnToInventory();
            }
            CollectionAssert.AreEquivalent(itemList, inventoryItemList);
        }
        [Category("ItemDetails")]
        [Test]
        public void InventoryItemDescription_ShouldMatchItemDetailDescription()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            var inventoryDescriptionList = inventoryPage.GetAllDescriptions();
            List<string> itemDescriptionList = new List<string>();
            foreach (var item in InventoryData.ItemNames)
            {
                var itemPage = inventoryPage.GoToItemPage(item);
                var getDescription = itemPage.GetDescription();
                itemDescriptionList.Add(getDescription);
                inventoryPage = itemPage.ReturnToInventory();
            }
            CollectionAssert.AreEquivalent(itemDescriptionList, inventoryDescriptionList);
        }
        [Category("ItemDetails")]
        [Test]
        public void InventoryItemPrice_ShouldMatchItemDetailPrice()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();
            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            var inventoryPricesList = inventoryPage.GetAllPrices();
            List<double> itemPricesList = new List<double>();
            foreach (var item in InventoryData.ItemNames)
            {
                var itemPage = inventoryPage.GoToItemPage(item);
                var getPrice = itemPage.GetPrice();
                itemPricesList.Add(getPrice);
                inventoryPage = itemPage.ReturnToInventory();
            }
            CollectionAssert.AreEquivalent(itemPricesList, inventoryPricesList);
        }
        [Category("Cart")]
        [Test]
        public void ItemPage_AddToCart_ShouldIncrementBadge()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();
            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            var itemPage = inventoryPage.GoToItemPage(InventoryData.Backpack);
            itemPage.AddToCart();
            Assert.That(itemPage.GetCartCount(), Is.EqualTo(1));
        }
        [Category("Cart")]
        [Test]
        public void ItemPage_RemoveFromCart_ShouldDecrementCartBadge()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();
            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            var itemPage = inventoryPage.GoToItemPage(InventoryData.Backpack);
            itemPage.AddToCart();
            Assert.That(itemPage.GetCartCount(), Is.EqualTo(1));
            itemPage.RemoveItem();
            Assert.That(itemPage.GetCartCount(), Is.EqualTo(0));

        }
        [Category("Navigation")]
        [Test]
        public void ItemPage_BackButton_ShouldReturnToInventoryPage()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();
            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            var itemPage = inventoryPage.GoToItemPage(InventoryData.Backpack);
            inventoryPage = itemPage.ReturnToInventory();
            Assert.That(inventoryPage.WaitForStablePage(), Is.True);
        }
        [Category("Authentication")]
        [Test]
        public void Logout_FromItemPage_ShouldReturnToLoginPage()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();
            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            var itemPage = inventoryPage.GoToItemPage(InventoryData.Backpack);
            itemPage.OpenMenu();
            loginPage = itemPage.Logout();
            Assert.That(loginPage.WaitForStablePage(), Is.True);
        }
    }
}
