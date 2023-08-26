using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace GameStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Organization - create Game object with image data
            Game game = new Game
            {
                GameId = 2,
                Name = "Game2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Organization - create mocked repository
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Game1" },
                game,
                new Game { GameId = 3, Name = "Game3" },
            }.AsQueryable());

            // Organization - create controller
            GameController controller = new GameController(mock.Object);

            // Act - calling the GetImage() action method
            ActionResult result = controller.GetImage(2);

            // Statement
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(game.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrive_Image_Data_For_Invalid_ID()
        {
            // Organization - create mocked repository
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Game1" },
                new Game { GameId = 2, Name = "Game2" },

            }.AsQueryable);

            // Organization - create controller
            GameController controller = new GameController(mock.Object);

            // Act - calling GetImage() action method
            ActionResult result = controller.GetImage(10);

            // Statement
            Assert.IsNull(result);
        }
    }
}
