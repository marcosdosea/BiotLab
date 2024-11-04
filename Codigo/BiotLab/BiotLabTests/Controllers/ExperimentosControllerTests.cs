using AutoMapper;
using BiotLabWeb.Controllers;
using BiotLabWeb.Models;
using Core.Service;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using BiotLabWeb.Mapper;

namespace BiotLabWeb.Controllers.Tests
{
    [TestClass()]
    public class ExperimentoControllerTests
    {
        private ExperimentoController _controller = null!;
        private Mock<IExperimentoService> _mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            _mockService = new Mock<IExperimentoService>();
            IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ExperimentoProfile())).CreateMapper();

            _mockService.Setup(service => service.GetAll()).Returns(GetTestExperimentos());
            _mockService.Setup(service => service.Get(It.IsAny<uint>())).Returns(GetTargetExperimento());
            _mockService.Setup(service => service.Create(It.IsAny<Experimento>())).Verifiable();
            _mockService.Setup(service => service.Update(It.IsAny<Experimento>())).Verifiable();
            _mockService.Setup(service => service.Delete(It.IsAny<uint>())).Verifiable();

            _controller = new ExperimentoController(_mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<ExperimentoViewModel>));
        }

        [TestMethod()]
        public void CreateTest_Valido()
        {
            // Arrange
            var newExperimento = new ExperimentoViewModel
            {
                Id = 2,
                Nome = "Experimento 2",
                DataInicio = DateTime.Now.ToString("dd/MM/yyyy"),
                DataFim = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"),
                Cepa = "Cepa Teste",
                Gaiolas = "Gaiola 1, Gaiola 2",
                IdPesquisadorNavigation = "1",
                Usoanestesicos = "Anestésico Teste"
            };

            // Act
            var result = _controller.Create(newExperimento);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            _mockService.Verify(service => service.Create(It.IsAny<Experimento>()), Times.Once);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = _controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ExperimentoViewModel));
        }

        [TestMethod()]
        public void EditTest_Valido()
        {
            // Arrange
            var editExperimento = new ExperimentoViewModel
            {
                Id = 1,
                Nome = "Experimento 1 Editado",
                DataInicio = DateTime.Now.ToString("dd/MM/yyyy"),
                DataFim = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"),
                Cepa = "Cepa Editada",
                Gaiolas = "Gaiola 1",
                IdPesquisadorNavigation = "1",
                Usoanestesicos = "Anestésico Editado"
            };

            // Act
            var result = _controller.Edit(1, editExperimento);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            _mockService.Verify(service => service.Update(It.IsAny<Experimento>()), Times.Once);
        }

        [TestMethod()]
        public void DeleteTest_Valido()
        {
            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ExperimentoViewModel));
        }

        [TestMethod()]
        public void DeleteConfirmedTest_Valido()
        {
            // Act
            var result = _controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            _mockService.Verify(service => service.Delete(It.IsAny<uint>()), Times.Once);
        }

        private IEnumerable<Experimento> GetTestExperimentos()
        {
            return new List<Experimento>
        {
            new Experimento
            {
                Id = 1,
                DataInicio = DateTime.Now.AddDays(-5),
                DataFim = DateTime.Now.AddDays(5),
                Cepa = "Cepa 1",
                IdPesquisador = 1,
                Gaiolas = new List<Gaiola>(), // Adicione se necessário
                Usoanestesicos = new List<Usoanestesico>() // Adicione se necessário
            },
            // Adicione mais experimentos de teste se necessário
        };
        }

        private Experimento GetTargetExperimento()
        {
            return new Experimento
            {
                Id = 1,
                DataInicio = DateTime.Now.AddDays(-5),
                DataFim = DateTime.Now.AddDays(5),
                Cepa = "Cepa 1",
                IdPesquisador = 1,
                Gaiolas = new List<Gaiola>(), // Adicione se necessário
                Usoanestesicos = new List<Usoanestesico>() // Adicione se necessário
            };
        }
    }
}
