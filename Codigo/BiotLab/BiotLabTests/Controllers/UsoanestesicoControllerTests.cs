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
    public class UsoanestesicoControllerTests
    {
        private UsoanestesicoController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IUsoanestesicoService>();
            var mockPesquisadorService = new Mock<IPesquisadorService>();
            var mockExperimentoService = new Mock<IExperimentoService>();
            var mockEntradaanestesicoService = new Mock<IEntradaanestesicoService>();

            IMapper mapper = MapperTestFactory.CreateMapper(new UsoanestesicoProfile());

            mockService.Setup(service => service.GetAll()).Returns(GetTestUsoanestesicos());
            mockService.Setup(service => service.Get(1)).Returns(GetTargetUsoanestesico());
            mockService.Setup(service => service.Create(It.IsAny<Usoanestesico>())).Verifiable();
            mockService.Setup(service => service.Update(It.IsAny<Usoanestesico>())).Verifiable();
            mockService.Setup(service => service.Delete(1)).Verifiable();

            mockPesquisadorService.Setup(service => service.GetAll()).Returns(GetTestPesquisadores());
            mockPesquisadorService.Setup(service => service.Buscar(1)).Returns(GetTargetPesquisador());

            mockExperimentoService.Setup(service => service.GetAll()).Returns(GetTestExperimentos());
            mockExperimentoService.Setup(service => service.Get(1)).Returns(GetTargetExperimento());

            mockEntradaanestesicoService.Setup(service => service.GetAll()).Returns(GetTestEntradasAnestesicos());
            mockEntradaanestesicoService.Setup(service => service.Get(1, 1)).Returns(GetTargetEntradaAnestesico());

            controller = new UsoanestesicoController(
                mockService.Object,
                mockPesquisadorService.Object,
                mockExperimentoService.Object,
                mockEntradaanestesicoService.Object,
                mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<UsoanestesicoViewModel>));

            var lista = (List<UsoanestesicoViewModel>)viewResult.ViewData.Model!;
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(UsoanestesicoViewModel));

            var usoanestesicoModel = (UsoanestesicoViewModel)viewResult.ViewData.Model!;
            Assert.AreEqual(10, usoanestesicoModel.Quantidade);
        }

        [TestMethod]
        public void CreateTest_Valido()
        {
            var result = controller.Create(GetNewUsoanestesico());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public void EditTest_Post_Valido()
        {
            var result = controller.Edit(1, GetTargetUsoanestesicoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public void DeleteTest_Post_Valido()
        {
            var result = controller.Delete(1, GetTargetUsoanestesicoModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        private static UsoanestesicoViewModel GetNewUsoanestesico()
        {
            return new UsoanestesicoViewModel
            {
                Id = 4,
                Quantidade = 15,
                Procedimento = "Procedimento Novo",
                Data = DateTime.Today,
                Cepa = "Cepa Nova",
                NumeroAnimais = 5,
                IdPesquisador = 1,
                IdExperimento = 1,
                IdEntrada = 1,
                IdAnestesico = 1
            };
        }

        private static Usoanestesico GetTargetUsoanestesico()
        {
            return new Usoanestesico
            {
                Id = 1,
                Quantidade = 10,
                Procedimento = "Procedimento 1",
                Data = DateTime.Today.AddDays(-1),
                Cepa = "Cepa 1",
                NumeroAnimais = 2,
                IdPesquisador = 1,
                IdExperimento = 1,
                IdEntrada = 1,
                IdAnestesico = 1,
                IdPesquisadorNavigation = GetTargetPesquisador(),
                IdExperimentoNavigation = GetTargetExperimento(),
                Entradaanestesico = GetTargetEntradaAnestesico()
            };
        }

        private static UsoanestesicoViewModel GetTargetUsoanestesicoModel()
        {
            return new UsoanestesicoViewModel
            {
                Id = 1,
                Quantidade = 10,
                Procedimento = "Procedimento 1",
                Data = DateTime.Today.AddDays(-1),
                Cepa = "Cepa 1",
                NumeroAnimais = 2,
                IdPesquisador = 1,
                IdExperimento = 1,
                IdEntrada = 1,
                IdAnestesico = 1
            };
        }

        private static IEnumerable<Usoanestesico> GetTestUsoanestesicos()
        {
            return new List<Usoanestesico>
            {
                GetTargetUsoanestesico(),
                new()
                {
                    Id = 2,
                    Quantidade = 20,
                    Procedimento = "Procedimento 2",
                    Data = DateTime.Today.AddDays(-2),
                    Cepa = "Cepa 2",
                    NumeroAnimais = 3,
                    IdPesquisador = 1,
                    IdExperimento = 1,
                    IdEntrada = 1,
                    IdAnestesico = 1,
                    IdPesquisadorNavigation = GetTargetPesquisador(),
                    IdExperimentoNavigation = GetTargetExperimento(),
                    Entradaanestesico = GetTargetEntradaAnestesico()
                },
                new()
                {
                    Id = 3,
                    Quantidade = 30,
                    Procedimento = "Procedimento 3",
                    Data = DateTime.Today.AddDays(-3),
                    Cepa = "Cepa 3",
                    NumeroAnimais = 4,
                    IdPesquisador = 1,
                    IdExperimento = 1,
                    IdEntrada = 1,
                    IdAnestesico = 1,
                    IdPesquisadorNavigation = GetTargetPesquisador(),
                    IdExperimentoNavigation = GetTargetExperimento(),
                    Entradaanestesico = GetTargetEntradaAnestesico()
                }
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

        private static Experimento GetTargetExperimento()
        {
            return new Experimento
            {
                Id = 1,
                Titulo = "Projeto 1",
                Cepa = "Cepa 1",
                DataInicio = DateTime.Today.AddDays(-10),
                DataFim = DateTime.Today.AddDays(10),
                IdPesquisador = 1
            };
        }

        private static IEnumerable<Experimento> GetTestExperimentos()
        {
            return new List<Experimento>
            {
                GetTargetExperimento()
            };
        }

        private static Entradaanestesico GetTargetEntradaAnestesico()
        {
            return new Entradaanestesico
            {
                IdEntrada = 1,
                IdAnestesico = 1,
                Lote = "Lote 1",
                IdEntradaNavigation = new Entradum
                {
                    Id = 1,
                    NumeroNotaFiscal = "NF001",
                    DataEntrada = DateTime.Today.AddDays(-5)
                },
                IdAnestesicoNavigation = new Anestesico
                {
                    Id = 1,
                    Nome = "Anestesico 1"
                }
            };
        }

        private static IEnumerable<Entradaanestesico> GetTestEntradasAnestesicos()
        {
            return new List<Entradaanestesico>
            {
                GetTargetEntradaAnestesico()
            };
        }
    }
}
