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
        private InstituicaoController controller = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IInstituicaoService>();

            IMapper mapper = MapperTestFactory.CreateMapper(new InstituicaoProfile());

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
            // Act
            var result = controller.DeleteConfirmed(1);

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
                Cnpj = "12345678000199",
                Cep = "12345-678",
                Cidade = "Cidade Nova",
                Estado = "SP",
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
                Cnpj = "98765432000199",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "SP",
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
                Cnpj = "98765432000199",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "SP",
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
                    Cnpj = "98765432000199",
                    Cep = "12345-678",
                    Cidade = "Cidade A",
                    Estado = "SP",
                    Telefone1 = "12345678",
                    Email = "instituicao1@email.com"
                },
                new Instituicao
                {
                    Id = 2,
                    Nome = "Instituição 2",
                    Cnpj = "12345678000188",
                    Cep = "23456-789",
                    Cidade = "Cidade B",
                    Estado = "RJ",
                    Telefone1 = "87654321",
                    Email = "instituicao2@email.com"
                },
                new Instituicao
                {
                    Id = 3,
                    Nome = "Instituição 3",
                    Cnpj = "12345678000177",
                    Cep = "34567-890",
                    Cidade = "Cidade C",
                    Estado = "MG",
                    Telefone1 = "12349876",
                    Email = "instituicao3@email.com"
                }
            };
        }
    }
}
