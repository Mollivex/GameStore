using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Moq.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GameStore.WebUI.Tests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Items()
        {
            // Organization - creating a few test games
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };
            Game game3 = new Game { GameId = 3, Name = "Game3" };
            Game game4 = new Game { GameId = 4, Name = "Game4" };

            // Organization - creating cart
            Cart cart = new Cart();

            // Action
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game3, 1);
            cart.AddItem(game4, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Statement
            Assert.AreEqual(results.Count(), 4);
            Assert.AreEqual(results[0].Game, game1);
            Assert.AreEqual(results[1].Game, game2);
            Assert.AreEqual(results[2].Game, game3);
            Assert.AreEqual(results[3].Game, game4);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Organization - creating a few test games
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };
            Game game3 = new Game { GameId = 3, Name = "Game3" };
            Game game4 = new Game { GameId = 4, Name = "Game4" };

            // Organization - creating cart
            Cart cart = new Cart();

            // Action - adding a few games to the cart
            cart.AddItem(game1, 2);
            cart.AddItem(game4, 2);
            cart.AddItem(game2, 2);
            cart.AddItem(game3, 1);
            cart.AddItem(game1, 1);
            cart.AddItem(game4, 3);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Game.GameId).ToList();

            // Statement
            Assert.AreEqual(results.Count(), 4);
            Assert.AreEqual(results[0].Quantity, 3); // 3 instances are added to cart
            Assert.AreEqual(results[1].Quantity, 2); // 2 instances are added to cart
            Assert.AreEqual(results[2].Quantity, 1);
            Assert.AreEqual(results[3].Quantity, 5); // 6 instances are added to cart
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Organization - creating a few test games
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };
            Game game3 = new Game { GameId = 3, Name = "Game3" };
            Game game4 = new Game { GameId = 4, Name = "Game4" };

            // Organization - creating cart
            Cart cart = new Cart();

            // Organization - adding a few games to the cart
            cart.AddItem(game1, 2);
            cart.AddItem(game4, 2);
            cart.AddItem(game2, 2);
            cart.AddItem(game3, 1);
            cart.AddItem(game1, 1);
            cart.AddItem(game4, 3);

            // Action
            cart.RemoveLine(game2);

            // Statement
            Assert.AreEqual(cart.Lines.Where(c => c.Game == game2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 3);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Organization - creating a few test games
            Game game1 = new Game { GameId = 1, Name = "Game1", Price = 100 };
            Game game2 = new Game { GameId = 2, Name = "Game2", Price = 75 };
            Game game3 = new Game { GameId = 3, Name = "Game3", Price = 50 };
            Game game4 = new Game { GameId = 4, Name = "Game4", Price = 25 };

            // Organization - creating cart
            Cart cart = new Cart();

            // Action
            cart.AddItem(game1, 2);
            cart.AddItem(game4, 2);
            cart.AddItem(game2, 2);
            cart.AddItem(game3, 1);
            cart.AddItem(game1, 1);
            cart.AddItem(game4, 3);
            decimal result = cart.ComputeTotalValue();

            // Statement
            Assert.AreEqual(result, 625);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Organization - creating a few test games
            Game game1 = new Game { GameId = 1, Name = "Game1", Price = 100 };
            Game game2 = new Game { GameId = 2, Name = "Game2", Price = 75 };
            Game game3 = new Game { GameId = 3, Name = "Game3", Price = 50 };
            Game game4 = new Game { GameId = 4, Name = "Game4", Price = 25 };

            // Organization - creating cart
            Cart cart = new Cart();

            // Action
            cart.AddItem(game1, 2);
            cart.AddItem(game4, 2);
            cart.AddItem(game2, 2);
            cart.AddItem(game3, 1);
            cart.AddItem(game1, 1);
            cart.AddItem(game4, 3);
            cart.Clear();

            // Statement
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        /// <summary>
        /// Verify adding to cart
        /// </summary>
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Organization - creating simulated repository
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game{ GameId = 1, Name = "Game1", Category = "Cat1"},
            }.AsQueryable());

            // Organization - creating cart
            Cart cart = new Cart();

            // Organization - creating controller
            CartController controller = new CartController(mock.Object);

            // Action - add game to cart
            controller.AddToCart(cart, 1, null);

            // Statement
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Game.GameId, 1);
        }

        /// <summary>
        /// After adding game to cart, goes to cart screen redirection
        /// </summary>
        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // Organization - creating simulated repository
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game{ GameId = 1, Name = "Game1", Category = "Cat1"},
            }.AsQueryable());

            // Organization - creating cart
            Cart cart = new Cart();

            // Organization - creating controller
            CartController controller = new CartController(mock.Object);

            // Action - add game to cart
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Statement
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        /// <summary>
        /// Check URL
        /// </summary>
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Organization - creating cart
            Cart cart = new Cart();

            // Organization - creating controller
            CartController target = new CartController(null);

            // Action - Index() action method call
            CartIndexViewModel result 
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Statement
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Organization - creating simulated order handler
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Orgranization - creating empty cart
            Cart cart = new Cart();

            // Organization - creating shipping details
            ShippingDetails shippingDetails = new ShippingDetails();

            // Organization - creating controller
            CartController controller = new CartController(null, mock.Object);

            // Action
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Statement - checking, that order isn't send to handler
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), 
                Times.Never());

            // Statement - checking, that method has returned default view
            Assert.AreEqual("", result.ViewName);

            // Statement - checking, that view has wrong model
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Organization - creating simulated order handler
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Organization - creating cart with the element
            Cart cart = new Cart();
            cart.AddItem(new Game(), 1);

            // Organization - creating controller
            CartController controller = new CartController(null, mock.Object);

            // Organization - adding error to model
            controller.ModelState.AddModelError("error", "error");

            // Action - trying to checkout
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Statement - checking that the order isn't passed to the handler
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Statement - checking, that method has returned default view
            Assert.AreEqual("", result.ViewName);

            // Statement - checking, that the wrong model is passed to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Organization - creating simulated order handler
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Organization - creating cart with the element
            Cart cart = new Cart();
            cart.AddItem(new Game(), 1);

            // Organization - creating controller
            CartController controller = new CartController(null, mock.Object);

            // Action - trying to checkout
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Statement - checking, that the order has been passed to the handler
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Statement - checking, that method returns view
            Assert.AreEqual("Completed", result.ViewName);

            // Statement - checking, that a valid model is being passed to the view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);


        }
    }
}