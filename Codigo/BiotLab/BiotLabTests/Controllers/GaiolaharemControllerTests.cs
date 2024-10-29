using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutoMapper;
using BiotLabWeb.Controllers;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiotLabWeb.Tests.Controllers
{
    [TestClass]
    public class GaiolaharemControllerTests
    {
        private static GaiolaharemController controller;

        [TestInitialize]
        public void Initialize()
        {
            // Configuração do AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Gaiolaharem, GaiolaharemViewModel>()
                    .ForMember(dest => dest.CodigoInternoGaiola, opt => opt.MapFrom(src => src.IdGaiolaNavigation.CodigoInterno))
                    .ForMember(dest => dest.CodigoInternoHarem, opt => opt.MapFrom(src => src.IdHaremNavigation.CodigoInterno))
                    .ForMember(dest => dest.NomePesquisador, opt => opt.MapFrom(src => src.IdPesquisadorNavigation.Nome))
                    .ReverseMap()
                    .ForMember(dest => dest.IdGaiolaNavigation, opt => opt.Ignore())
                    .ForMember(dest => dest.IdHaremNavigation, opt => opt.Ignore())
                    .ForMember(dest => dest.IdPesquisadorNavigation, opt => opt.Ignore());
            });
            IMapper mapper = config.CreateMapper();

            // Mock dos serviços
            var mockGaiolaharemService = new Mock<IGaiolaharemService>();
            var mockGaiolaService = new Mock<IGaiolaService>();
            var mockHaremService = new Mock<IHaremService>();
            var mockPesquisadorService = new Mock<IPesquisadorService>();

            // Configuração dos mocks
            mockGaiolaharemService.Setup(service => service.GetAll())
                .Returns(GetTestGaiolaharems());
            mockGaiolaharemService.Setup(service => service.Get(1, 1))
                .Returns(GetTargetGaiolaharem());
            mockGaiolaharemService.Setup(service => service.Create(It.IsAny<Gaiolaharem>()));
            mockGaiolaharemService.Setup(service => service.Update(It.IsAny<Gaiolaharem>()));
            mockGaiolaharemService.Setup(service => service.Delete(1, 1));

            mockGaiolaService.Setup(service => service.GetAll())
                .Returns(GetTestGaiolas());
            mockHaremService.Setup(service => service.GetAll())
                .Returns(GetTestHarems());
            mockPesquisadorService.Setup(service => service.GetAll())
                .Returns(GetTestPesquisadores());

            // Instância do controlador
            controller = new GaiolaharemController(
                mockGaiolaharemService.Object,
                mockGaiolaService.Object,
                mockHaremService.Object,
                mockPesquisadorService.Object,
                mapper);
        }

        [TestMethod]
        public void IndexTest()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            var model = viewResult.Model as IEnumerable<GaiolaharemViewModel>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public void DetailsTest_Existing()
        {
            // Act
            var result = controller.Details(1, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            var model = viewResult.Model as GaiolaharemViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1U, model.IdGaiola);
            Assert.AreEqual(1U, model.IdHarem);
        }

        [TestMethod]
        public void DetailsTest_NotFound()
        {
            // Arrange
            var mockService = new Mock<IGaiolaharemService>();
            mockService.Setup(service => service.Get(It.IsAny<uint>(), It.IsAny<uint>()))
                .Returns((Gaiolaharem)null);

            var controller = new GaiolaharemController(
                mockService.Object,
                null,
                null,
                null,
                null);

            // Act
            var result = controller.Details(999, 999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void CreateTest_Get()
        {
            // Act
            var result = controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void CreateTest_Post_Valid()
        {
            // Arrange
            var model = GetNewGaiolaharemViewModel();

            // Act
            var result = controller.Create(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod]
        public void EditTest_Get()
        {
            // Act
            var result = controller.Edit(1, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            var model = viewResult.Model as GaiolaharemViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1U, model.IdGaiola);
            Assert.AreEqual(1U, model.IdHarem);
        }

        [TestMethod]
        public void EditTest_Post_Valid()
        {
            // Arrange
            var model = GetTargetGaiolaharemViewModel();

            // Act
            var result = controller.Edit(1, 1, model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod]
        public void DeleteTest_Get()
        {
            // Act
            var result = controller.Delete(1, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            var model = viewResult.Model as GaiolaharemViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1U, model.IdGaiola);
            Assert.AreEqual(1U, model.IdHarem);
        }

        [TestMethod]
        public void DeleteTest_Post()
        {
            // Act
            var result = controller.DeleteConfirmed(1, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        // Métodos auxiliares para obter dados de teste

        private Gaiolaharem GetTargetGaiolaharem()
        {
            return new Gaiolaharem
            {
                IdGaiola = 1,
                IdHarem = 1,
                DataPovoamento = DateTime.Now.AddDays(-10),
                IdPesquisador = 1,
                IdGaiolaNavigation = new Gaiola
                {
                    Id = 1,
                    CodigoInterno = "Gaiola001"
                },
                IdHaremNavigation = new Harem
                {
                    Id = 1,
                    CodigoInterno = "Harem001"
                },
                IdPesquisadorNavigation = new Pesquisador
                {
                    Id = 1,
                    Nome = "Dr. Silva"
                }
            };
        }

        private IEnumerable<Gaiolaharem> GetTestGaiolaharems()
        {
            return new List<Gaiolaharem>
            {
                new Gaiolaharem
                {
                    IdGaiola = 1,
                    IdHarem = 1,
                    DataPovoamento = DateTime.Now.AddDays(-10),
                    IdPesquisador = 1,
                    IdGaiolaNavigation = new Gaiola
                    {
                        Id = 1,
                        CodigoInterno = "Gaiola001"
                    },
                    IdHaremNavigation = new Harem
                    {
                        Id = 1,
                        CodigoInterno = "Harem001"
                    },
                    IdPesquisadorNavigation = new Pesquisador
                    {
                        Id = 1,
                        Nome = "Dr. Silva"
                    }
                },
                new Gaiolaharem
                {
                    IdGaiola = 2,
                    IdHarem = 2,
                    DataPovoamento = DateTime.Now.AddDays(-5),
                    IdPesquisador = 2,
                    IdGaiolaNavigation = new Gaiola
                    {
                        Id = 2,
                        CodigoInterno = "Gaiola002"
                    },
                    IdHaremNavigation = new Harem
                    {
                        Id = 2,
                        CodigoInterno = "Harem002"
                    },
                    IdPesquisadorNavigation = new Pesquisador
                    {
                        Id = 2,
                        Nome = "Dr. Souza"
                    }
                }
            };
        }

        private GaiolaharemViewModel GetNewGaiolaharemViewModel()
        {
            return new GaiolaharemViewModel
            {
                IdGaiola = 3,
                IdHarem = 3,
                DataPovoamento = DateTime.Now,
                IdPesquisador = 1
            };
        }

        private GaiolaharemViewModel GetTargetGaiolaharemViewModel()
        {
            return new GaiolaharemViewModel
            {
                IdGaiola = 1,
                IdHarem = 1,
                DataPovoamento = DateTime.Now.AddDays(-2),
                IdPesquisador = 2
            };
        }

        private IEnumerable<Gaiola> GetTestGaiolas()
        {
            return new List<Gaiola>
            {
                new Gaiola { Id = 1, CodigoInterno = "Gaiola001" },
                new Gaiola { Id = 2, CodigoInterno = "Gaiola002" },
                new Gaiola { Id = 3, CodigoInterno = "Gaiola003" }
            };
        }

        private IEnumerable<Harem> GetTestHarems()
        {
            return new List<Harem>
            {
                new Harem { Id = 1, CodigoInterno = "Harem001" },
                new Harem { Id = 2, CodigoInterno = "Harem002" },
                new Harem { Id = 3, CodigoInterno = "Harem003" }
            };
        }

        private IEnumerable<Pesquisador> GetTestPesquisadores()
        {
            return new List<Pesquisador>
            {
                new Pesquisador { Id = 1, Nome = "Dr. Silva" },
                new Pesquisador { Id = 2, Nome = "Dr. Souza" },
                new Pesquisador { Id = 3, Nome = "Dr. Oliveira" }
            };
        }
    }
}
