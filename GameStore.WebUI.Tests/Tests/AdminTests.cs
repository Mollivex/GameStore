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

        [TestMethod]
        public void Can_Edit_Game()
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
            Game game1 = controller.Edit(1).ViewData.Model as Game;
            Game game2 = controller.Edit(2).ViewData.Model as Game;
            Game game3 = controller.Edit(3).ViewData.Model as Game;

            // Assert
            Assert.AreEqual(1, game1.GameId);
            Assert.AreEqual(2, game2.GameId);
            Assert.AreEqual(3, game3.GameId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Game()
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
            Game result = controller.Edit(6).ViewData.Model as Game;

            //Assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Organization - creating simulated repository
            Mock<IGameRepository> mock = new Mock<IGameRepository>();

            // Organization - creating controller
            AdminController controller = new AdminController(mock.Object);

            // Organization - creating Game object
            Game game = new Game { Name = "Test" };

            // Act - trying to save item
            ActionResult result = controller.Edit(game);

            // Statement - checking, that repository has been accessed
            mock.Verify(m => m.SaveGame(game));

            // Statement - checking method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Organization - creating simualted repository
            Mock<IGameRepository> mock = new Mock<IGameRepository> ();

            // Organization - creating controller
            AdminController controller = new AdminController(mock.Object);

            // Organization - creating Game object
            Game game = new Game { Name = "Test" };

            // Organization - adding an error to model state
            controller.ModelState.AddModelError("error", "error");

            // Act - trying to save item
            ActionResult result = controller.Edit(game);

            // Statement - checking, that repository has NOT been accessed
            mock.Verify(m => m.SaveGame(It.IsAny<Game>()), Times.Never());

            // Statement - checking method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Games()
        {
            // Organization - creating Game object
            Game game = new Game { GameId = 2, Name = "Game2" };

            // Organization - creating simulated repository
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Game1" },
                new Game { GameId = 2, Name = "Game2" },
                new Game { GameId = 3, Name = "Game3" },
                new Game { GameId = 4, Name = "Game4" },
                new Game { GameId = 5, Name = "Game5" }
            });

            // Organization - controller creating
            AdminController controller = new AdminController(mock.Object);

            // Act - delete game
            controller.Delete(game.GameId);

            // Statement - checking that delete method has been passed
            // for the correct Game object
            mock.Verify(m => m.DeleteGame(game.GameId));
        }
    }
}
