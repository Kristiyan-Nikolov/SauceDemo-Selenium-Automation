using NUnit.Framework.Legacy;
using SauceDemo.Data;
using SauceDemo.SauceDemoPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.SauceDemoTests
{
    public class InventoryTests : BaseTests
    {
        [Category("Cart")]
        [Test]
        public void AddMultipleItemsToCart_ShouldIncreaseCartCounter()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.AddItem(InventoryData.Backpack);
            inventoryPage.AddItem(InventoryData.BikeLight);
            int cartNumber = inventoryPage.GetCartCount();
            Assert.That(cartNumber, Is.EqualTo(2));
        }
        [Category("Cart")]
        [Test]
        public void RemovingItemsFromInventory_ShouldDecreaseCartCounter()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.AddItem(InventoryData.Backpack);
            inventoryPage.AddItem(InventoryData.BikeLight);
            inventoryPage.RemoveItem(InventoryData.Backpack);
            inventoryPage.RemoveItem(InventoryData.BikeLight);
            int cartNumber = inventoryPage.GetCartCount();
            Assert.That(cartNumber, Is.EqualTo(0));
        }
        [Category("Sorting")]
        [Test]
        public void Sort_ByName_AZ_ShouldOrderItemsAlphabetically()
        {
            var loginPage = new LoginPage(driver); 
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.SortBy(InventoryData.SortAZ);
            var actualResult = inventoryPage.GetItemNames();
            var expectedResult = actualResult.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(expectedResult, expectedResult);
        }
        [Category("Sorting")]
        [Test]
        public void Sort_ByName_ZA_ShouldOrderItemsReverseAlphabetically()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.SortBy(InventoryData.SortZA);
            var actualResult = inventoryPage.GetItemNames();
            var expectedResult = actualResult.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(expectedResult, expectedResult);
        }
        [Category("Sorting")]
        [Test]
        public void Sort_ByPrice_LowHigh_ShouldOrderItemsByPrice()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.SortBy(InventoryData.SortLowHigh);
            var actualResult = inventoryPage.GetPrices();
            var expectedResult = actualResult.OrderBy(x =>x).ToList();
            CollectionAssert.AreEqual(expectedResult, expectedResult);
        }
        [Category("Sorting")]
        [Test]
        public void Sort_ByPrice_HighLow_ShouldOrderItemsByPrice()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.SortBy(InventoryData.SortHighLow);
            var actualResult = inventoryPage.GetPrices();
            var expectedResult = actualResult.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(expectedResult, expectedResult);
        }
        [Category("Inventory")]
        [Test]
        public void Inventory_ShouldOpenEachItemDetail_WhenClicked()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
           
            for (int i = 0; i < InventoryData.ItemNames.Length; i++)
            {
                var itemPage = inventoryPage.GoToItemPage(InventoryData.ItemNames[i]);
                var actualName = itemPage.GetItemName();
                Assert.That(InventoryData.ItemNames[i], Is.EqualTo(actualName));
                
                inventoryPage = itemPage.ReturnToInventory();
            }

        }
        [Category("Cart")]
        [Test]
        public void ItemPage_AddRemove_ShouldToggleButtonAndCartCount()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            var itemPage = inventoryPage.GoToItemPage(InventoryData.FleeceJacket);
            Assert.That(itemPage.WaitForStablePage(), Is.True);
            itemPage.AddToCart();
            Assert.That(itemPage.GetCartCount(), Is.EqualTo(1));
            itemPage.RemoveItem();
            Assert.That(itemPage.GetCartCount(), Is.EqualTo(0));
            inventoryPage = itemPage.ReturnToInventory();
            Assert.That(inventoryPage.IsOpen(), Is.True);
            Assert.That(inventoryPage.IsAddToCartButtonVisible(InventoryData.FleeceJacket), Is.True);

        }
        [Category("Cart")]
        [Test]
        public void ItemState_ShouldPersist_AfterLogoutAndRelogin()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.AddItem(InventoryData.FleeceJacket);
            inventoryPage.AddItem(InventoryData.Backpack);
            var fleeceJacketAddToCartVisible = inventoryPage.IsAddToCartButtonVisible(InventoryData.FleeceJacket);
            var BackpackAddToCartVisible = inventoryPage.IsAddToCartButtonVisible(InventoryData.FleeceJacket);

            Assert.Multiple(() =>
            {
                Assert.That(inventoryPage.GetCartCount(), Is.EqualTo(2));
                Assert.That(fleeceJacketAddToCartVisible, Is.False);
                Assert.That(BackpackAddToCartVisible, Is.False);
            });
            inventoryPage.OpenMenu();
            loginPage = inventoryPage.Logout();

            Assert.That(loginPage.WaitForStablePage(), Is.True);
            inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            fleeceJacketAddToCartVisible = inventoryPage.IsAddToCartButtonVisible(InventoryData.FleeceJacket);
            BackpackAddToCartVisible = inventoryPage.IsAddToCartButtonVisible(InventoryData.FleeceJacket);

            Assert.Multiple(() =>
            {
                Assert.That(inventoryPage.GetCartCount(), Is.EqualTo(2));
                Assert.That(fleeceJacketAddToCartVisible, Is.False);
                Assert.That(BackpackAddToCartVisible, Is.False);
            });

        }

    }
}
