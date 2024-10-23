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
                Nome = "Experimento Novo",
                Cnpj = "1234567890001",
                Cep = "12345-678",
                Cidade = "Cidade Nova",
                Estado = "Estado",
                Telefone1 = "12345678",
                Email = "email@Aluno.com"
            };
        }

        private Experimento GetTargetExperimento()
        {
            return new Experimento
            {
                Id = 1,
                Nome = "Experimento 1",
                Cnpj = "9876543210001",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "Estado A",
                Telefone1 = "12345678",
                Email = "Aluno1@email.com"
            };
        }

        private ExperimentoViewModel GetTargetExperimentoModel()
        {
            return new ExperimentoViewModel
            {
                Id = 1,
                Nome = "Experimento 1",
                Cnpj = "9876543210001",
                Cep = "12345-678",
                Cidade = "Cidade A",
                Estado = "Estado A",
                Telefone1 = "12345678",
                Email = "Aluno1@email.com"
            };
        }

        private IEnumerable<Experimento> GetTestExperimento()
        {
            return new List<Experimento>
            {
                new Experimento
                {
                    Id = 1,
                    Nome = "Experimento 1",
                    Cnpj = "9876543210001",
                    Cep = "12345-678",
                    Cidade = "Cidade A",
                    Estado = "Estado A",
                    Telefone1 = "12345678",
                    Email = "Aluno1@email.com"
                },
                new Experimento
                {
                    Id = 2,
                    Nome = "Experimento 2",
                    Cnpj = "1234567890002",
                    Cep = "23456-789",
                    Cidade = "Cidade B",
                    Estado = "Estado B",
                    Telefone1 = "87654321",
                    Email = "Aluno2@email.com"
                },
                new Experimento
                {
                    Id = 3,
                    Nome = "Experimento 3",
                    Cnpj = "1234567890003",
                    Cep = "34567-890",
                    Cidade = "Cidade C",
                    Estado = "Estado C",
                    Telefone1 = "12349876",
                    Email = "Aluno3@email.com"
                }
            };
        }
    }
}
