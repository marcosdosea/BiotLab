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
    public class ExperimentoControllerTests
    {
        private static ExperimentoController controller;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IExperimentoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new ExperimentoProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestExperimento());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetExperimento());
            mockService.Setup(service => service.Create(It.IsAny<Experimento>()))
                .Verifiable();
            mockService.Setup(service => service.Update(It.IsAny<Experimento>()))
                .Verifiable();
            mockService.Setup(service => service.Delete(1))
                .Verifiable();

            controller = new ExperimentoController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<ExperimentoViewModel>));

            var lista = (List<ExperimentoViewModel>)viewResult.ViewData.Model;
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
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ExperimentoViewModel));

            var experimentoModel = (ExperimentoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Experimento 1", experimentoModel.Nome);
        }

        [TestMethod()]
        public void CreateTest_Valido()
        {
            // Act
            var result = controller.Create(GetNewExperimento());

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
            var result = controller.Edit(1, GetTargetExperimentoModel());

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
            var experimentoModel = GetTargetExperimentoModel(); // Obtém o modelo para deletar

            // Act
            var result = controller.Delete(1, experimentoModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        // Métodos auxiliares
        private ExperimentoViewModel GetNewExperimento()
        {
            return new ExperimentoViewModel
            {
                Id = 4,
                Nome = "Novo Experimento",
                DataInicio = DateTime.Now.ToString("dd.MM.yyyy"), // Formato esperado
                DataFim = DateTime.Now.AddDays(5).ToString("dd.MM.yyyy"), // Formato esperado
                Cepa = "Cepa C",
                IdPesquisadorNavigation = "3", // Como string, se necessário, ou ajuste conforme seu modelo
                Usoanestesicos = "Anestesico A",
                Gaiolas = "5"
            };
        }

        private Experimento GetTargetExperimento()
        {
            return new Experimento
            {
                Id = 5,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(5),
                Cepa = "Cepa C",
                IdPesquisador = 3, // Ajustado para uint
                // Gaiolas e Usoanestesicos não são preenchidos aqui, pois são collections
            };
        }

        private ExperimentoViewModel GetTargetExperimentoModel()
        {
            return new ExperimentoViewModel
            {
                Id = 1,
                Nome = "Experimento 1",
                DataInicio = DateTime.Now.ToString("dd.MM.yyyy"), // Formato esperado
                DataFim = DateTime.Now.AddDays(5).ToString("dd.MM.yyyy"), // Formato esperado
                Cepa = "Cepa A",
                IdPesquisadorNavigation = "3", // Ajuste conforme seu modelo
                Usoanestesicos = "Anestesico A",
                Gaiolas = "2"
            };
        }

        private IEnumerable<Experimento> GetTestExperimento()
        {
            return new List<Experimento>
            {
                new Experimento
                {
                    Id = 1,
                    DataInicio = DateTime.Now,
                    DataFim = DateTime.Now.AddDays(5),
                    Cepa = "Cepa A",
                    IdPesquisador = 3, // Ajustado para uint
                },
                new Experimento
                {
                    Id = 2,
                    DataInicio = DateTime.Now,
                    DataFim = DateTime.Now.AddDays(5),
                    Cepa = "Cepa B",
                    IdPesquisador = 3, // Ajustado para uint
                },
                new Experimento
                {
                    Id = 3,
                    DataInicio = DateTime.Now,
                    DataFim = DateTime.Now.AddDays(5),
                    Cepa = "Cepa C",
                    IdPesquisador = 3, // Ajustado para uint
                }
            };
        }
    }
}
