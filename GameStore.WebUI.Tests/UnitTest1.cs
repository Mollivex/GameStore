using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.Models;
using GameStore.WebUI.HtmlHelpers;

namespace GameStore.WebUI.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Organization (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 8, Name = "Game1" },
                new Game {GameId = 9, Name = "Game2" },
                new Game {GameId = 10, Name = "Game3" },
                new Game {GameId = 11, Name = "Game4" },
                new Game {GameId = 12, Name = "Game5" },
                new Game {GameId = 13, Name = "Game6" },
                new Game {GameId = 14, Name = "Game7" },
                new Game {GameId = 15, Name = "Game8" },
                new Game {GameId = 16, Name = "Game9" },
                new Game {GameId = 17, Name = "Game10" },
                new Game {GameId = 18, Name = "Game11" },
                new Game {GameId = 19, Name = "Game12" },
            });

            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Action (act)
            GamesListViewModel result = (GamesListViewModel)controller.List(null, 2).Model;

            // Statement (assert)
            List<Game> games = result.Games.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Game11");
            Assert.AreEqual(games[1].Name, "Game12");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Organization - definition HTML additional method - it's need to use extenstion method
            HtmlHelper myHelper = null;

            // Organization - creating PagingInfo object
            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemPerPage = 10
            };

            // Organization - delegate set up with lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Action
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Statement
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                            + @"<a class="" btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                            + @"<a class=""btn btn-default"" href=""Page3"">3<a/a>",
                            result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Organization (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 8, Name = "Game1" },
                new Game {GameId = 9, Name = "Game2" },
                new Game {GameId = 10, Name = "Game3" },
                new Game {GameId = 11, Name = "Game4" },
                new Game {GameId = 12, Name = "Game5" },
                new Game {GameId = 13, Name = "Game6" },
                new Game {GameId = 14, Name = "Game7" },
                new Game {GameId = 15, Name = "Game8" },
                new Game {GameId = 16, Name = "Game9" },
                new Game {GameId = 17, Name = "Game10" },
                new Game {GameId = 18, Name = "Game11" },
                new Game {GameId = 19, Name = "Game12" }
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Act
            GamesListViewModel result = (GamesListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 2);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            // Organization (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 8, Name = "Game1", Category = "Cat1" },
                new Game {GameId = 9, Name = "Game2", Category = "Cat2" },
                new Game {GameId = 10, Name = "Game3", Category = "Cat1" },
                new Game {GameId = 11, Name = "Game4", Category = "Cat3" },
                new Game {GameId = 12, Name = "Game5", Category = "Cat2" },
                new Game {GameId = 13, Name = "Game6", Category = "Cat1" },
                new Game {GameId = 14, Name = "Game7", Category = "Cat4" },
                new Game {GameId = 15, Name = "Game8", Category = "Cat1" },
                new Game {GameId = 16, Name = "Game9", Category = "Cat4" },
                new Game {GameId = 17, Name = "Game10", Category = "Cat3" },
                new Game {GameId = 18, Name = "Game11", Category = "Cat1" },
                new Game {GameId = 19, Name = "Game12", Category = "Cat3" }
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Game> result = ((GamesListViewModel)controller.List("Cat2", 1).Model).Games.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Game2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[0].Name == "Game5" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Organization - creating simualated storage
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 8, Name = "Game1", Category = "Simulator" },
                new Game {GameId = 9, Name = "Game2", Category = "First-person shooter" },
                new Game {GameId = 10, Name = "Game3", Category = "Simulator" },
                new Game {GameId = 11, Name = "Game4", Category = "Racing" },
                new Game {GameId = 12, Name = "Game5", Category = "First-person shooter" },
                new Game {GameId = 13, Name = "Game6", Category = "Simulator" },
                new Game {GameId = 14, Name = "Game7", Category = "Action-adventure" },
                new Game {GameId = 15, Name = "Game8", Category = "Simulator" },
                new Game {GameId = 16, Name = "Game9", Category = "Action-adventure" },
                new Game {GameId = 17, Name = "Game10", Category = "Racing" },
                new Game {GameId = 18, Name = "Game11", Category = "Simulator" },
                new Game {GameId = 19, Name = "Game12", Category = "Racing" }
            });

            // Organization - creating controller
            NavController target = new NavController(mock.Object);

            // Action -  getting the categories set
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Statement
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Simulator");
            Assert.AreEqual(results[1], "Racing");
            Assert.AreEqual(results[2], "Action-adventure");
            Assert.AreEqual(results[3], "First-person shooter");
        }

        [TestMethod]
        public void Indicates_Selected_Categories()
        {
            // Organization - creating simulated storage
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new Game[]
            {
                new Game {GameId = 8, Name = "Game1", Category = "Simulator" },
                new Game {GameId = 9, Name = "Game2", Category = "First-person shooter" },
                new Game {GameId = 10, Name = "Game3", Category = "Simulator" },
                new Game {GameId = 11, Name = "Game4", Category = "Racing" },
                new Game {GameId = 12, Name = "Game5", Category = "First-person shooter" },
                new Game {GameId = 13, Name = "Game6", Category = "Simulator" },
                new Game {GameId = 14, Name = "Game7", Category = "Action-adventure" },
                new Game {GameId = 15, Name = "Game8", Category = "Simulator" },
                new Game {GameId = 16, Name = "Game9", Category = "Action-adventure" },
                new Game {GameId = 17, Name = "Game10", Category = "Racing" },
                new Game {GameId = 18, Name = "Game11", Category = "Simulator" },
                new Game {GameId = 19, Name = "Game12", Category = "Racing" }
            });

            // Organization - creating controller
            NavController target = new NavController(mock.Object);

            // Organization - selected category definition
            string categoryToSelect = "Simulator";

            // Action
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Statement
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            // Organization (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 8, Name = "Game1", Category = "Cat1" },
                new Game {GameId = 9, Name = "Game2", Category = "Cat2" },
                new Game {GameId = 10, Name = "Game3", Category = "Cat1" },
                new Game {GameId = 11, Name = "Game4", Category = "Cat3" },
                new Game {GameId = 12, Name = "Game5", Category = "Cat2" },
                new Game {GameId = 13, Name = "Game6", Category = "Cat1" },
                new Game {GameId = 14, Name = "Game7", Category = "Cat4" },
                new Game {GameId = 15, Name = "Game8", Category = "Cat1" },
                new Game {GameId = 16, Name = "Game9", Category = "Cat4" },
                new Game {GameId = 17, Name = "Game10", Category = "Cat3" },
                new Game {GameId = 18, Name = "Game11", Category = "Cat1" },
                new Game {GameId = 19, Name = "Game12", Category = "Cat3" }
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Action - testing the counter of products for different categories
            int res1 = ((GamesListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((GamesListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((GamesListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int res4 = ((GamesListViewModel)controller.List("Cat4").Model).PagingInfo.TotalItems;
            int resAll = ((GamesListViewModel)controller.List("null").Model).PagingInfo.TotalItems;

            // Statement
            Assert.AreEqual(res1, 5);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 3);
            Assert.AreEqual(res4, 2);
            Assert.AreEqual(resAll, 12);
        }
    }
    
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
            Assert.AreEqual(cart.Lines.Count(), 2);
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
    }
}
