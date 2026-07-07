using AutoMapper;
using BiotLabWeb.Mapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BiotLabWeb.Controllers.Tests
{
    [TestClass]
    public class ExperimentoControllerTests
    {
        private ExperimentoController controller = null!;
        private Mock<IExperimentoService> mockService = null!;

        [TestInitialize]
        public void Initialize()
        {
            mockService = new Mock<IExperimentoService>();
            var mockPesquisadorService = new Mock<IPesquisadorService>();
            IMapper mapper = MapperTestFactory.CreateMapper(new ExperimentoProfile());

            mockService.Setup(service => service.GetAll()).Returns(GetTestExperimentos());
            mockService.Setup(service => service.Get(It.IsAny<uint>())).Returns(GetTargetExperimento());
            mockService.Setup(service => service.Create(It.IsAny<Experimento>(), It.IsAny<IEnumerable<uint>>())).Verifiable();
            mockService.Setup(service => service.Update(It.IsAny<Experimento>(), It.IsAny<IEnumerable<uint>>())).Verifiable();
            mockService.Setup(service => service.Delete(It.IsAny<uint>())).Verifiable();

            mockPesquisadorService.Setup(service => service.GetAll()).Returns(GetTestPesquisadores());
            mockPesquisadorService.Setup(service => service.Buscar(1)).Returns(GetTargetPesquisador());

            controller = new ExperimentoController(mockService.Object, mockPesquisadorService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<ExperimentoViewModel>));
        }

        [TestMethod]
        public void CreateTest_Valido()
        {
            var newExperimento = new ExperimentoViewModel
            {
                Id = 2,
                Titulo = "Projeto Teste",
                DataInicio = DateTime.Today,
                DataFim = DateTime.Today.AddDays(1),
                Cepa = "Cepa Teste",
                IdsPesquisadores = new List<uint> { 1 }
            };

            var result = controller.Create(newExperimento);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            mockService.Verify(service => service.Create(It.IsAny<Experimento>(), It.IsAny<IEnumerable<uint>>()), Times.Once);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ExperimentoViewModel));
        }

        [TestMethod]
        public void EditTest_Valido()
        {
            var editExperimento = new ExperimentoViewModel
            {
                Id = 1,
                Titulo = "Projeto Editado",
                DataInicio = DateTime.Today,
                DataFim = DateTime.Today.AddDays(1),
                Cepa = "Cepa Editada",
                IdsPesquisadores = new List<uint> { 1 }
            };

            var result = controller.Edit(1, editExperimento);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            mockService.Verify(service => service.Update(It.IsAny<Experimento>(), It.IsAny<IEnumerable<uint>>()), Times.Once);
        }

        [TestMethod]
        public void DeleteTest_Valido()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ExperimentoViewModel));
        }

        [TestMethod]
        public void DeleteConfirmedTest_Valido()
        {
            var result = controller.Delete(1, new ExperimentoViewModel { Id = 1 });

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            mockService.Verify(service => service.Delete(It.IsAny<uint>()), Times.Once);
        }

        private static IEnumerable<Experimento> GetTestExperimentos()
        {
            return new List<Experimento>
            {
                GetTargetExperimento()
            };
        }

        private static Experimento GetTargetExperimento()
        {
            return new Experimento
            {
                Id = 1,
                Titulo = "Projeto 1",
                DataInicio = DateTime.Today.AddDays(-5),
                DataFim = DateTime.Today.AddDays(5),
                Cepa = "Cepa 1",
                IdPesquisador = 1,
                ExperimentoPesquisadores = new List<ExperimentoPesquisador>
                {
                    new ExperimentoPesquisador
                    {
                        IdExperimento = 1,
                        IdPesquisador = 1,
                        IdPesquisadorNavigation = GetTargetPesquisador()
                    }
                },
                Gaiolas = new List<Gaiola>(),
                Usoanestesicos = new List<Usoanestesico>()
            };
        }

        private static Pesquisador GetTargetPesquisador()
        {
            return new Pesquisador
            {
                Id = 1,
                Nome = "Pesquisador 1"
            };
        }

        private static IEnumerable<Pesquisador> GetTestPesquisadores()
        {
            return new List<Pesquisador>
            {
                GetTargetPesquisador()
            };
        }
    }
}
