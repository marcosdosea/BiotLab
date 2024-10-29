using AutoMapper;
using Moq;
using Core.Service;
using BiotLabWeb.Mapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using BiotLabWeb.Models;

namespace BiotLabWeb.Controllers.Tests
{
    [TestClass()]
    public class BioterioControllerTests
    {
        private static BioterioController controller;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IBioterioService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new BioterioProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestBioterio());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetBioterio());
            mockService.Setup(service => service.Create(It.IsAny<Bioterio>()))
                .Verifiable();
            mockService.Setup(service => service.Update(It.IsAny<Bioterio>()))
                .Verifiable();
            mockService.Setup(service => service.Delete(1))
                .Verifiable();

            controller = new BioterioController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<BioterioViewModel>));

            var lista = (List<BioterioViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(BioterioViewModel));

            var bioterioModel = (BioterioViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Bioterio 1", bioterioModel.Nome);
        }

        [TestMethod()]
        public void CreateTest_Valido()
        {
            // Act
            var result = controller.Create(GetNewBioterio());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            // Act
            var result = controller.Edit(1, GetTargetBioterioModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            // Arrange
            var bioterioModel = GetTargetBioterioModel(); // Obtém o modelo para deletar

            // Act
            var result = controller.Delete(1, bioterioModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        // Métodos auxiliares
        private BioterioViewModel GetNewBioterio()
        {
            return new BioterioViewModel
            {
                Id = 4,
                Nome = "Bioterio Novo",
                Cep = "12345-678",
                Cidade = "Cidade Nova",
                Estado = "Estado",
                Telefone1 = "12345678",
                Email = "email@bioterio.com"
            };
        }

        private Bioterio GetTargetBioterio()
        {
            return new Bioterio
            {
                Id = 1,
                Nome = "Bioterio 1",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "Estado A",
                Telefone1 = "12345678",
                Email = "bioterio1@email.com"
            };
        }

        private BioterioViewModel GetTargetBioterioModel()
        {
            return new BioterioViewModel
            {
                Id = 1,
                Nome = "Bioterio 1",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "Estado A",
                Telefone1 = "12345678",
                Email = "bioterio1@email.com"
            };
        }

        private IEnumerable<Bioterio> GetTestBioterio()
        {
            return new List<Bioterio>
            {
                new Bioterio
                {
                    Id = 1,
                    Nome = "Bioterio 1",
                    Cep = "12345-678",
                    Cidade = "Cidade A",
                    Estado = "Estado A",
                    Telefone1 = "12345678",
                    Email = "bioterio1@email.com"
                },
                new Bioterio
                {
                    Id = 2,
                    Nome = "Bioterio 2",
                    Cep = "23456-789",
                    Cidade = "Cidade B",
                    Estado = "Estado B",
                    Telefone1 = "87654321",
                    Email = "bioterio2@email.com"
                },
                new Bioterio
                {
                    Id = 3,
                    Nome = "Bioterio 3",
                    Cep = "34567-890",
                    Cidade = "Cidade C",
                    Estado = "Estado C",
                    Telefone1 = "12349876",
                    Email = "bioterio3@email.com"
                }
            };
        }
    }
}
