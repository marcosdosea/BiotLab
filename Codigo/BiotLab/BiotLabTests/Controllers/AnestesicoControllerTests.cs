using AutoMapper;
using Moq;
using Core.Service;
using BiotLabWeb.Mapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using BiotLabWeb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BiotLabWeb.Controllers.Tests
{
    [TestClass()]
    public class AnestesicoControllerTests
    {
        private static AnestesicoController controller;
        private Mock<IAnestesicoService> mockService;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            mockService = new Mock<IAnestesicoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AnestesicoProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestAnestesicos());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetAnestesico());
            mockService.Setup(service => service.Create(It.IsAny<Anestesico>()))
                .Verifiable();
            mockService.Setup(service => service.Update(It.IsAny<Anestesico>()))
                .Verifiable();
            mockService.Setup(service => service.Delete(1))
                .Verifiable();

            controller = new AnestesicoController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<AnestesicoViewModel>));

            var lista = (List<AnestesicoViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(2, lista.Count);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(AnestesicoViewModel));

            var anestesicoModel = (AnestesicoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Anestesico A", anestesicoModel.Nome);
        }

        [TestMethod()]
        public void CreateTest_Valido()
        {
            // Act
            var result = controller.Create(GetNewAnestesico());

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
            var result = controller.Edit(1, GetTargetAnestesicoModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            // Act
            var result = controller.Delete(1, GetTargetAnestesicoModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        // Métodos auxiliares
        private AnestesicoViewModel GetNewAnestesico()
        {
            return new AnestesicoViewModel
            {
                Id = 3,
                Nome = "Anestesico C",
                Marca = "Marca C",
                Concentracao = 1.5M
            };
        }

        private Anestesico GetTargetAnestesico()
        {
            return new Anestesico
            {
                Id = 1,
                Nome = "Anestesico A",
                Marca = "Marca A",
                Concentracao = 0.5M
            };
        }

        private AnestesicoViewModel GetTargetAnestesicoModel()
        {
            return new AnestesicoViewModel
            {
                Id = 1,
                Nome = "Anestesico A",
                Marca = "Marca A",
                Concentracao = 0.5M
            };
        }

        private IEnumerable<Anestesico> GetTestAnestesicos()
        {
            return new List<Anestesico>
            {
                new Anestesico
                {
                    Id = 1,
                    Nome = "Anestesico A",
                    Marca = "Marca A",
                    Concentracao = 0.5M
                },
                new Anestesico
                {
                    Id = 2,
                    Nome = "Anestesico B",
                    Marca = "Marca B",
                    Concentracao = 1.0M
                }
            };
        }
    }
}
