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
    public class InstituicaoControllerTests
    {
        private static InstituicaoController controller;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IInstituicaoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new InstituicaoProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestInstituicoes());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetInstituicao());
            mockService.Setup(service => service.Create(It.IsAny<Instituicao>()))
                .Verifiable();
            mockService.Setup(service => service.Update(It.IsAny<Instituicao>()))
                .Verifiable();
            mockService.Setup(service => service.Delete(1))
                .Verifiable();

            controller = new InstituicaoController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<InstituicaoViewModel>));

            var lista = (List<InstituicaoViewModel>)viewResult.ViewData.Model;
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
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(InstituicaoViewModel));

            var instituicaoModel = (InstituicaoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Instituição 1", instituicaoModel.Nome);
        }

        [TestMethod()]
        public void CreateTest_Valido()
        {
            // Act
            var result = controller.Create(GetNewInstituicao());

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
            var result = controller.Edit(1, GetTargetInstituicaoModel());

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
            var instituicaoModel = GetTargetInstituicaoModel(); // Obtém o modelo para deletar

            // Act
            var result = controller.Delete(1, instituicaoModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        // Métodos auxiliares
        private InstituicaoViewModel GetNewInstituicao()
        {
            return new InstituicaoViewModel
            {
                Id = 4,
                Nome = "Instituição Nova",
                Cnpj = "1234567890001",
                Cep = "12345-678",
                Cidade = "Cidade Nova",
                Estado = "Estado",
                Telefone1 = "12345678",
                Email = "email@instituicao.com"
            };
        }

        private Instituicao GetTargetInstituicao()
        {
            return new Instituicao
            {
                Id = 1,
                Nome = "Instituição 1",
                Cnpj = "9876543210001",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "Estado A",
                Telefone1 = "12345678",
                Email = "instituicao1@email.com"
            };
        }

        private InstituicaoViewModel GetTargetInstituicaoModel()
        {
            return new InstituicaoViewModel
            {
                Id = 1,
                Nome = "Instituição 1",
                Cnpj = "9876543210001",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "Estado A",
                Telefone1 = "12345678",
                Email = "instituicao1@email.com"
            };
        }

        private IEnumerable<Instituicao> GetTestInstituicoes()
        {
            return new List<Instituicao>
            {
                new Instituicao
                {
                    Id = 1,
                    Nome = "Instituição 1",
                    Cnpj = "9876543210001",
                    Cep = "12345-678",
                    Cidade = "Cidade A",
                    Estado = "Estado A",
                    Telefone1 = "12345678",
                    Email = "instituicao1@email.com"
                },
                new Instituicao
                {
                    Id = 2,
                    Nome = "Instituição 2",
                    Cnpj = "1234567890002",
                    Cep = "23456-789",
                    Cidade = "Cidade B",
                    Estado = "Estado B",
                    Telefone1 = "87654321",
                    Email = "instituicao2@email.com"
                },
                new Instituicao
                {
                    Id = 3,
                    Nome = "Instituição 3",
                    Cnpj = "1234567890003",
                    Cep = "34567-890",
                    Cidade = "Cidade C",
                    Estado = "Estado C",
                    Telefone1 = "12349876",
                    Email = "instituicao3@email.com"
                }
            };
        }
    }
}
