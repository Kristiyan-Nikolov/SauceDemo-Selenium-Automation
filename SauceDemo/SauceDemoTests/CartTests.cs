using SauceDemo.SauceDemoPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SauceDemo.Data;

namespace SauceDemo.SauceDemoTests
{
    public class CartTests : BaseTests
    {
        [Category("Cart")]
        [Test]
        public void AddItems_ToCart_ShouldUpdateBadge()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.AddItem(InventoryData.Backpack);
            inventoryPage.AddItem(InventoryData.BikeLight);
            
            var cartPage = inventoryPage.OpenCart();
            int countItems = cartPage.GetCartCount();
            
            Assert.That(countItems, Is.EqualTo(2));
        }
        [Category("Cart")]
        [Test]
        public void RemoveItems_FromCart_ShouldUpdateBadge()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.AddItem(InventoryData.Backpack);

            var cartPage = inventoryPage.OpenCart();
            cartPage.RemoveItem(InventoryData.Backpack);
            int countItems = cartPage.GetCartCount();

            Assert.That(countItems, Is.EqualTo(0));
        }
        [Category("Smoke")]
        [Category("Cart")]
        [Test]
        public void AddItem_FromInventory_ShouldAppearInCart()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.AddItem(InventoryData.Backpack);

            var cartPage = inventoryPage.OpenCart();
            var itemName = cartPage.GetItemName(InventoryData.Backpack);

            Assert.That(itemName, Is.EqualTo(InventoryData.Backpack));
        }
        [Category("Cart")]
        [Test]
        public void RemoveItem_FromCart_ShouldAllowReaddingFromInventory()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);

            string itemName = InventoryData.Backpack;

            inventoryPage.AddItem(itemName);

            var cartPage = inventoryPage.OpenCart();
            cartPage.RemoveItem(itemName);
            inventoryPage = cartPage.ReturnToInventory();
            inventoryPage.AddItem(itemName);
            cartPage = inventoryPage.OpenCart();
            int countItems = cartPage.GetCartCount();
            Assert.That(countItems, Is.EqualTo(1));
        }
        // State transition test
        [Category("Cart")]
        [Test]
        public void AddAndRemoveItem_Repeatedly_ShouldMaintainCorrectCartState()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            string itemName = InventoryData.Backpack;
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"---Cycle {i + 1} ---");
                inventoryPage.AddItem(itemName);
                var cartPage = inventoryPage.OpenCart();
                int count1 = cartPage.GetCartCount();

                Assert.That(count1, Is.EqualTo(1), $"Cart count incorrect in cycle {i + 1} during ADD."); // (We are all) one spark

                cartPage.RemoveItem(itemName);

                int count2 = cartPage.GetCartCount();
                Assert.That(count2, Is.EqualTo(0), $"Cart count incorrect in cycle {i + 1} during REMOVE."); // Eyes full of wonder
                inventoryPage = cartPage.ReturnToInventory();
                Assert.That(inventoryPage.IsOpen(), Is.True);
                

            }
        }
        [Category("Cart")]
        [Test]
        public void CartItemNames_ShouldMatchInventoryNames()
        {
            var loginPage = new LoginPage(driver);
            loginPage.OpenPage();

            var inventoryPage = loginPage.Login(UserData.StandardUser, UserData.Password);
            inventoryPage.AddItem(InventoryData.Backpack);
            inventoryPage.AddItem(InventoryData.FleeceJacket);
            var cartPage = inventoryPage.OpenCart();
            var cartBackpackName = cartPage.GetItemName(InventoryData.Backpack);
            var cartFleeceJacketName = cartPage.GetItemName(InventoryData.FleeceJacket);
            Assert.Multiple(() =>
            {
                Assert.That(cartBackpackName, Is.EqualTo(InventoryData.Backpack));
                Assert.That(cartFleeceJacketName, Is.EqualTo(InventoryData.FleeceJacket));
            });
        }
    }
}
