using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            // Organization - creating simulated repository
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Game1"},
                new Game { GameId = 2, Name = "Game2"},
                new Game { GameId = 3, Name = "Game3"},
                new Game { GameId = 4, Name = "Game4"},
                new Game { GameId = 5, Name = "Game5"},
            });

            // Organization - creating controller
            AdminController controller = new AdminController(mock.Object);

            // Act
            List<Game> result = ((IEnumerable<Game>)controller.Index()
                .ViewData.Model).ToList();

            // Statement
            Assert.AreEqual(result.Count, 5);
            Assert.AreEqual("Game1", result[0].Name);
            Assert.AreEqual("Game2", result[1].Name);
            Assert.AreEqual("Game3", result[2].Name);
        }
    }
}
