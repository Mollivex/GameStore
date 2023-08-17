using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using System.Configuration;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { Name = "SIMS 4", Price = 1499 },
                new Game { Name = "Grand Theft Auto 5", Price = 2599 },
                new Game { Name = "Mafia 2", Price = 1899 },
                new Game { Name = "Battlefield 3", Price = 1099 },
                new Game { Name = "Call of duty: Modern Warfare 2", Price = 1299 },
                new Game { Name = "Rome II: Total war", Price = 1999 },
            });
            kernel.Bind<IGameRepository>().ToConstant(mock.Object);
        }
    }
}