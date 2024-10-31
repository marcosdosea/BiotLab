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
        private Mock<IExperimentoService> mockService; // Mantenha uma referência ao mock

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IExperimentoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new ExperimentoProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestExperimentos());
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
            var result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(List<ExperimentoViewModel>));

            var lista = (List<ExperimentoViewModel>)result.ViewData.Model;
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(ExperimentoViewModel));

            var experimentoModel = (ExperimentoViewModel)result.ViewData.Model;
            Assert.AreEqual("Cepa A", experimentoModel.Cepa);
        }

        [TestMethod()]
        public void CreateTest_Valido()
        {
            var experimentoModel = GetNewExperimento();

            var result = controller.Create(experimentoModel) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.IsNull(result.ControllerName);
            Assert.AreEqual("Index", result.ActionName);

            // Verifica se o método Create foi chamado uma vez
            mockService.Verify(service => service.Create(It.IsAny<Experimento>()), Times.Once);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            var experimentoModel = GetTargetExperimentoModel();

            var result = controller.Edit(1, experimentoModel) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.IsNull(result.ControllerName);
            Assert.AreEqual("Index", result.ActionName);

            // Verifica se o método Update foi chamado uma vez
            mockService.Verify(service => service.Update(It.IsAny<Experimento>()), Times.Once);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            var experimentoModel = GetTargetExperimentoModel();

            var result = controller.Delete(1, experimentoModel) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.IsNull(result.ControllerName);
            Assert.AreEqual("Index", result.ActionName);

            // Verifica se o método Delete foi chamado uma vez
            mockService.Verify(service => service.Delete(1), Times.Once);
        }

        // Métodos auxiliares
        private ExperimentoViewModel GetNewExperimento()
        {
            return new ExperimentoViewModel
            {
                Id = 4,
                DataInicio = DateTime.Now.ToString("dd.MM.yyyy"),
                DataFim = DateTime.Now.AddDays(5).ToString("dd.MM.yyyy"),
                Cepa = "Cepa C",
                IdPesquisadorNavigation = "3",
                Usoanestesicos = "Anestesico A",
                Gaiolas = "5"
            };
        }

        private Experimento GetTargetExperimento()
        {
            return new Experimento
            {
                Id = 1,
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(5),
                Cepa = "Cepa A",
                IdPesquisador = 3,
            };
        }

        private ExperimentoViewModel GetTargetExperimentoModel()
        {
            return new ExperimentoViewModel
            {
                Id = 1,
                DataInicio = DateTime.Now.ToString("dd.MM.yyyy"),
                DataFim = DateTime.Now.AddDays(5).ToString("dd.MM.yyyy"),
                Cepa = "Cepa A",
                IdPesquisadorNavigation = "3",
                Usoanestesicos = "Anestesico A",
                Gaiolas = "2"
            };
        }

        private IEnumerable<Experimento> GetTestExperimentos()
        {
            return new List<Experimento>
            {
                new Experimento
                {
                    Id = 1,
                    DataInicio = DateTime.Now,
                    DataFim = DateTime.Now.AddDays(5),
                    Cepa = "Cepa A",
                    IdPesquisador = 3,
                },
                new Experimento
                {
                    Id = 2,
                    DataInicio = DateTime.Now,
                    DataFim = DateTime.Now.AddDays(5),
                    Cepa = "Cepa B",
                    IdPesquisador = 3,
                },
                new Experimento
                {
                    Id = 3,
                    DataInicio = DateTime.Now,
                    DataFim = DateTime.Now.AddDays(5),
                    Cepa = "Cepa C",
                    IdPesquisador = 3,
                }
            };
        }
    }
}
