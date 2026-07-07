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
    public class BioterioControllerTests
    {
        private BioterioController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IBioterioService>();
            var mockInstituicaoService = new Mock<IInstituicaoService>();

            IMapper mapper = MapperTestFactory.CreateMapper(new BioterioProfile());

            mockService.Setup(service => service.GetAll()).Returns(GetTestBioterios());
            mockService.Setup(service => service.Get(1)).Returns(GetTargetBioterio());
            mockService.Setup(service => service.Create(It.IsAny<Bioterio>())).Verifiable();
            mockService.Setup(service => service.Update(It.IsAny<Bioterio>())).Verifiable();
            mockService.Setup(service => service.Delete(1)).Verifiable();

            mockInstituicaoService.Setup(service => service.GetAll()).Returns(GetTestInstituicoes());
            mockInstituicaoService.Setup(service => service.Get(1)).Returns(GetTargetInstituicao());

            controller = new BioterioController(mockService.Object, mockInstituicaoService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<BioterioViewModel>));

            var lista = (List<BioterioViewModel>)viewResult.ViewData.Model!;
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(BioterioViewModel));

            var bioterioModel = (BioterioViewModel)viewResult.ViewData.Model!;
            Assert.AreEqual("Bioterio 1", bioterioModel.Nome);
        }

        [TestMethod]
        public void CreateTest_Valido()
        {
            var result = controller.Create(GetNewBioterio());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public void EditTest_Post_Valido()
        {
            var result = controller.Edit(1, GetTargetBioterioModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public void DeleteTest_Post_Valido()
        {
            var result = controller.Delete(1, GetTargetBioterioModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        private static BioterioViewModel GetNewBioterio()
        {
            return new BioterioViewModel
            {
                Id = 4,
                Nome = "Bioterio Novo",
                Cep = "12345-678",
                Cidade = "Cidade Nova",
                Estado = "SP",
                Telefone1 = "12345678",
                Email = "email@bioterio.com",
                IdInstituicao = 1
            };
        }

        private static Bioterio GetTargetBioterio()
        {
            return new Bioterio
            {
                Id = 1,
                Nome = "Bioterio 1",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "SP",
                Telefone1 = "12345678",
                Email = "bioterio1@email.com",
                IdInstituicao = 1
            };
        }

        private static BioterioViewModel GetTargetBioterioModel()
        {
            return new BioterioViewModel
            {
                Id = 1,
                Nome = "Bioterio 1",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "SP",
                Telefone1 = "12345678",
                Email = "bioterio1@email.com",
                IdInstituicao = 1
            };
        }

        private static IEnumerable<Bioterio> GetTestBioterios()
        {
            return new List<Bioterio>
            {
                GetTargetBioterio(),
                new()
                {
                    Id = 2,
                    Nome = "Bioterio 2",
                    Cep = "23456-789",
                    Cidade = "Cidade B",
                    Estado = "RJ",
                    Telefone1 = "87654321",
                    Email = "bioterio2@email.com",
                    IdInstituicao = 1
                },
                new()
                {
                    Id = 3,
                    Nome = "Bioterio 3",
                    Cep = "34567-890",
                    Cidade = "Cidade C",
                    Estado = "MG",
                    Telefone1 = "12349876",
                    Email = "bioterio3@email.com",
                    IdInstituicao = 1
                }
            };
        }

        private static Instituicao GetTargetInstituicao()
        {
            return new Instituicao
            {
                Id = 1,
                Nome = "Instituicao 1"
            };
        }

        private static IEnumerable<Instituicao> GetTestInstituicoes()
        {
            return new List<Instituicao>
            {
                GetTargetInstituicao()
            };
        }
    }
}
