using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;

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
            IEnumerable<Game> result = (IEnumerable<Game>)controller.List(2).Model;

            // Statement (assert)
            List<Game> games = result.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Game11");
            Assert.AreEqual(games[1].Name, "Game12");
        }
    }
}
