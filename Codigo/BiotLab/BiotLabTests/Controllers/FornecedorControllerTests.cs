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
    public class FornecedorControllerTests
    {
        private static FornecedorController controller;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IFornecedorService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new FornecedorProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestFornecedors());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetFornecedor());
            mockService.Setup(service => service.Create(It.IsAny<Fornecedor>()))
                .Verifiable();
            mockService.Setup(service => service.Update(It.IsAny<Fornecedor>()))
                .Verifiable();
            mockService.Setup(service => service.Delete(1))
                .Verifiable();

            controller = new FornecedorController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<FornecedorViewModel>));

            var lista = (List<FornecedorViewModel>)viewResult.ViewData.Model;
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
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(FornecedorViewModel));

            var fornecedorModel = (FornecedorViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Fornecedor 1", fornecedorModel.Nome);
        }

        [TestMethod()]
        public void CreateTest_Valido()
        {
            // Act
            var result = controller.Create(GetNewFornecedor());

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
            var result = controller.Edit(1, GetTargetFornecedorModel());

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
            var fornecedorModel = GetTargetFornecedorModel(); // Obtém o modelo para deletar

            // Act
            var result = controller.Delete(1, fornecedorModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        // Métodos auxiliares
        private FornecedorViewModel GetNewFornecedor()
        {
            return new FornecedorViewModel
            {
                Id = 4,
                Nome = "Fornecedor Novo",
                Cnpj = "1234567890001",
                Cep = "12345-678",
                Cidade = "Cidade Nova",
                Estado = "Estado",
                Telefone1 = "12345678",
                Email = "email@fornecedor.com"
            };
        }

        private Fornecedor GetTargetFornecedor()
        {
            return new Fornecedor
            {
                Id = 1,
                Nome = "Fornecedor 1",
                Cnpj = "9876543210001",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "Estado A",
                Telefone1 = "12345678",
                Email = "fornecedor1@email.com"
            };
        }

        private FornecedorViewModel GetTargetFornecedorModel()
        {
            return new FornecedorViewModel
            {
                Id = 1,
                Nome = "Fornecedor 1",
                Cnpj = "9876543210001",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "Estado A",
                Telefone1 = "12345678",
                Email = "fornecedor1@email.com"
            };
        }

        private IEnumerable<Fornecedor> GetTestFornecedors()
        {
            return new List<Fornecedor>
            {
                new Fornecedor
                {
                    Id = 1,
                    Nome = "Fornecedor 1",
                    Cnpj = "9876543210001",
                    Cep = "12345-678",
                    Cidade = "Cidade A",
                    Estado = "Estado A",
                    Telefone1 = "12345678",
                    Email = "fornecedor1@email.com"
                },
                new Fornecedor
                {
                    Id = 2,
                    Nome = "Fornecedor 2",
                    Cnpj = "1234567890002",
                    Cep = "23456-789",
                    Cidade = "Cidade B",
                    Estado = "Estado B",
                    Telefone1 = "87654321",
                    Email = "fornecedor2@email.com"
                },
                new Fornecedor
                {
                    Id = 3,
                    Nome = "Fornecedor 3",
                    Cnpj = "1234567890003",
                    Cep = "34567-890",
                    Cidade = "Cidade C",
                    Estado = "Estado C",
                    Telefone1 = "12349876",
                    Email = "fornecedor3@email.com"
                }
            };
        }
    }
}
