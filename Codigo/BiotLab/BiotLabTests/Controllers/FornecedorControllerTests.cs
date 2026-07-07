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
    public class FornecedorControllerTests
    {
        private FornecedorController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IFornecedorService>();
            var mockInstituicaoService = new Mock<IInstituicaoService>();

            IMapper mapper = MapperTestFactory.CreateMapper(new FornecedorProfile());

            mockService.Setup(service => service.GetAll()).Returns(GetTestFornecedores());
            mockService.Setup(service => service.Get(1)).Returns(GetTargetFornecedor());
            mockService.Setup(service => service.Create(It.IsAny<Fornecedor>())).Verifiable();
            mockService.Setup(service => service.Update(It.IsAny<Fornecedor>())).Verifiable();
            mockService.Setup(service => service.Delete(1)).Verifiable();

            mockInstituicaoService.Setup(service => service.GetAll()).Returns(GetTestInstituicoes());
            mockInstituicaoService.Setup(service => service.Get(1)).Returns(GetTargetInstituicao());

            controller = new FornecedorController(mockService.Object, mockInstituicaoService.Object, mapper);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<FornecedorViewModel>));

            var lista = (List<FornecedorViewModel>)viewResult.ViewData.Model!;
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(FornecedorViewModel));

            var fornecedorModel = (FornecedorViewModel)viewResult.ViewData.Model!;
            Assert.AreEqual("Fornecedor 1", fornecedorModel.Nome);
        }

        [TestMethod]
        public void CreateTest_Valido()
        {
            var result = controller.Create(GetNewFornecedor());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public void EditTest_Post_Valido()
        {
            var result = controller.Edit(1, GetTargetFornecedorModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public void DeleteTest_Post_Valido()
        {
            var result = controller.Delete(1, GetTargetFornecedorModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        private static FornecedorViewModel GetNewFornecedor()
        {
            return new FornecedorViewModel
            {
                Id = 4,
                Nome = "Fornecedor Novo",
                Cnpj = "12345678000199",
                Cep = "12345-678",
                Cidade = "Cidade Nova",
                Estado = "SP",
                Telefone1 = "12345678",
                Email = "email@fornecedor.com",
                IdInstituicao = 1
            };
        }

        private static Fornecedor GetTargetFornecedor()
        {
            return new Fornecedor
            {
                Id = 1,
                Nome = "Fornecedor 1",
                Cnpj = "98765432000199",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "SP",
                Telefone1 = "12345678",
                Email = "fornecedor1@email.com",
                IdInstituicao = 1
            };
        }

        private static FornecedorViewModel GetTargetFornecedorModel()
        {
            return new FornecedorViewModel
            {
                Id = 1,
                Nome = "Fornecedor 1",
                Cnpj = "98765432000199",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "SP",
                Telefone1 = "12345678",
                Email = "fornecedor1@email.com",
                IdInstituicao = 1
            };
        }

        private static IEnumerable<Fornecedor> GetTestFornecedores()
        {
            return new List<Fornecedor>
            {
                GetTargetFornecedor(),
                new()
                {
                    Id = 2,
                    Nome = "Fornecedor 2",
                    Cnpj = "12345678000188",
                    Cep = "23456-789",
                    Cidade = "Cidade B",
                    Estado = "RJ",
                    Telefone1 = "87654321",
                    Email = "fornecedor2@email.com",
                    IdInstituicao = 1
                },
                new()
                {
                    Id = 3,
                    Nome = "Fornecedor 3",
                    Cnpj = "12345678000177",
                    Cep = "34567-890",
                    Cidade = "Cidade C",
                    Estado = "MG",
                    Telefone1 = "12349876",
                    Email = "fornecedor3@email.com",
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
