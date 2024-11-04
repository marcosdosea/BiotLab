using AutoMapper;
using Moq;
using Core.Service;
using BiotLabWeb.Mapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using BiotLabWeb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BiotLabWeb.Controllers.Tests
{
    [TestClass]
    public class EntradaanestesicoControllerTests
    {
        private static EntradaanestesicoController controller = null!;
        private static IMapper mapper = null!;
        private static Mock<IEntradaanestesicoService> mockService = new Mock<IEntradaanestesicoService>();
        private static Mock<IAnestesicosService> mockAnestesicoService = new Mock<IAnestesicosService>();
        private static Mock<IEntradumService> mockEntradumService = new Mock<IEntradumService>();

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new EntradaanestesicoProfile())).CreateMapper();
            mockService = new Mock<IEntradaanestesicoService>();
            mockAnestesicoService = new Mock<IAnestesicosService>();
            mockEntradumService = new Mock<IEntradumService>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new EntradaanestesicoProfile()));
            mapper = config.CreateMapper();

            // Configurando os serviços mock
            mockService.Setup(service => service.GetAll())
                .Returns(GetTestEntradaanestesicos());
            mockService.Setup(service => service.Get(1, 1))
                .Returns(GetTargetEntradaanestesico());
            mockService.Setup(service => service.Create(It.IsAny<Entradaanestesico>()))
                .Verifiable();
            mockService.Setup(service => service.Update(It.IsAny<Entradaanestesico>()))
                .Verifiable();
            mockService.Setup(service => service.Delete(1, 1))
                .Verifiable();

            mockAnestesicoService.Setup(service => service.GetAll())
                .Returns(GetTestAnestesicos());
            mockEntradumService.Setup(service => service.GetAll())
                .Returns(GetTestEntradas());

            controller = new EntradaanestesicoController(
                mockService.Object,
                mockAnestesicoService.Object,
                mockEntradumService.Object,
                mapper);
        }

        [TestMethod]
        public void IndexTest()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(IEnumerable<EntradaanestesicoViewModel>));

            var lista = (IEnumerable<EntradaanestesicoViewModel>)viewResult.Model;
            Assert.AreEqual(2, lista.Count());
        }

        [TestMethod]
        public void DetailsTest()
        {
            // Act
            var result = controller.Details(1, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(EntradaanestesicoViewModel));

            var model = (EntradaanestesicoViewModel)viewResult.Model;
            Assert.AreEqual(1U, model.IdEntrada);
            Assert.AreEqual(1U, model.IdAnestesico);
            Assert.AreEqual("Lote001", model.Lote);
        }

        [TestMethod]
        public void CreateTest_Get()
        {
            // Act
            var result = controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.ViewData["Anestesicos"]);
            Assert.IsNotNull(viewResult.ViewData["Entradas"]);
        }

        [TestMethod]
        public void CreateTest_Post_ValidModel()
        {
            // Arrange
            var newModel = GetNewEntradaanestesicoViewModel();

            // Act
            var result = controller.Create(newModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
            mockService.Verify(service => service.Create(It.IsAny<Entradaanestesico>()), Times.Once);
        }

        [TestMethod]
        public void CreateTest_Post_InvalidModel()
        {
            // Arrange
            var invalidModel = new EntradaanestesicoViewModel
            {
                IdEntrada = 0, // ID inválido
                IdAnestesico = 3,
                Quantidade = 15,
                Lote = "Lote003",
                ValorUnitario = 200,
                SubTotal = 3000
            };
            controller.ModelState.AddModelError("IdEntrada", "O ID da entrada é obrigatório.");

            // Act
            var result = controller.Create(invalidModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.AreEqual(invalidModel, viewResult.Model);
            Assert.IsNotNull(viewResult.ViewData["Anestesicos"]);
            Assert.IsNotNull(viewResult.ViewData["Entradas"]);
            mockService.Verify(service => service.Create(It.IsAny<Entradaanestesico>()), Times.Never);
        }

        [TestMethod]
        public void EditTest_Post()
        {
            // Arrange
            var updatedModel = GetTargetEntradaanestesicoViewModel();

            // Act
            var result = controller.Edit(1, 1, updatedModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
            mockService.Verify(service => service.Update(It.IsAny<Entradaanestesico>()), Times.Once);
        }

        [TestMethod]
        public void DeleteTest_Post()
        {
            // Act
            var result = controller.DeleteConfirmed(1, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
            mockService.Verify(service => service.Delete(1, 1), Times.Once);
        }

        // Métodos auxiliares para dados de teste
        private Entradaanestesico GetTargetEntradaanestesico()
        {
            return new Entradaanestesico
            {
                IdEntrada = 1,
                IdAnestesico = 1,
                Quantidade = 10,
                Lote = "Lote001",
                ValorUnitario = 50,
                SubTotal = 500,
                IdAnestesicoNavigation = new Anestesico
                {
                    Id = 1,
                    Nome = "Anestésico A",
                    Marca = "Marca A"
                },
                IdEntradaNavigation = new Entradum
                {
                    Id = 1,
                    DataEntrada = System.DateTime.Now.AddDays(-1),
                    NumeroNotaFiscal = "NF001"
                }
            };
        }

        private IEnumerable<Entradaanestesico> GetTestEntradaanestesicos()
        {
            return new List<Entradaanestesico>
            {
                new Entradaanestesico
                {
                    IdEntrada = 1,
                    IdAnestesico = 1,
                    Quantidade = 10,
                    Lote = "Lote001",
                    ValorUnitario = 50,
                    SubTotal = 500,
                    IdAnestesicoNavigation = new Anestesico
                    {
                        Id = 1,
                        Nome = "Anestésico A",
                        Marca = "Marca A"
                    },
                    IdEntradaNavigation = new Entradum
                    {
                        Id = 1,
                        DataEntrada = System.DateTime.Now.AddDays(-1),
                        NumeroNotaFiscal = "NF001"
                    }
                },
                new Entradaanestesico
                {
                    IdEntrada = 1,
                    IdAnestesico = 2,
                    Quantidade = 5,
                    Lote = "Lote002",
                    ValorUnitario = 100,
                    SubTotal = 500,
                    IdAnestesicoNavigation = new Anestesico
                    {
                        Id = 2,
                        Nome = "Anestésico B",
                        Marca = "Marca B"
                    },
                    IdEntradaNavigation = new Entradum
                    {
                        Id = 1,
                        DataEntrada = System.DateTime.Now.AddDays(-1),
                        NumeroNotaFiscal = "NF001"
                    }
                }
            };
        }

        private EntradaanestesicoViewModel GetNewEntradaanestesicoViewModel()
        {
            return new EntradaanestesicoViewModel
            {
                IdEntrada = 2,
                IdAnestesico = 3,
                Quantidade = 15,
                Lote = "Lote003",
                ValorUnitario = 200,
                SubTotal = 3000
            };
        }

        private EntradaanestesicoViewModel GetTargetEntradaanestesicoViewModel()
        {
            return new EntradaanestesicoViewModel
            {
                IdEntrada = 1,
                IdAnestesico = 1,
                Quantidade = 20,
                Lote = "Lote001",
                ValorUnitario = 50,
                SubTotal = 1000
            };
        }

        private IEnumerable<Anestesico> GetTestAnestesicos()
        {
            return new List<Anestesico>
            {
                new Anestesico { Id = 1, Nome = "Anestésico A", Marca = "Marca A" },
                new Anestesico { Id = 2, Nome = "Anestésico B", Marca = "Marca B" },
                new Anestesico { Id = 3, Nome = "Anestésico C", Marca = "Marca C" },
            };
        }

        private IEnumerable<Entradum> GetTestEntradas()
            {
                return new List<Entradum>
            {
                new Entradum { Id = 1, DataEntrada = System.DateTime.Now.AddDays(-1), NumeroNotaFiscal = "NF001" },
                new Entradum { Id = 2, DataEntrada = System.DateTime.Now.AddDays(-2), NumeroNotaFiscal = "NF002" }
            };
            }
        }
    }
