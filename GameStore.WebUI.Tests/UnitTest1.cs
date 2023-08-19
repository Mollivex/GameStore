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
            Mock<IGameRepository> mock = new Mock<IGameRepository>);
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
    }
}
