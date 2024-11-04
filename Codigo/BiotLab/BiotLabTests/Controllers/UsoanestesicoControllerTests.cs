using AutoMapper;
using Moq;
using Core.Service;
using BiotLabWeb.Mapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using BiotLabWeb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BiotLabWeb.Controllers
{
    [TestClass()]
    public class UsoanestesicoControllerTests
    {
        private static UsoanestesicoController controller;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IUsoanestesicoService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new UsoanestesicoProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestUsoanestesicos());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetUsoanestesico());
            mockService.Setup(service => service.Create(It.IsAny<Usoanestesico>()))
                .Verifiable();
            mockService.Setup(service => service.Update(It.IsAny<Usoanestesico>()))
                .Verifiable();
            mockService.Setup(service => service.Delete(1))
                .Verifiable();

            controller = new UsoanestesicoController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<UsoanestesicoViewModel>));

            var lista = (List<UsoanestesicoViewModel>)viewResult.ViewData.Model;
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
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(UsoanestesicoViewModel));

            var usoanestesicoModel = (UsoanestesicoViewModel)viewResult.ViewData.Model;
            Assert.AreEqual(10, usoanestesicoModel.Quantidade); // Supondo que a quantidade do teste seja 10
        }

        [TestMethod()]
        public void CreateTest_Valido()
        {
            // Act
            var result = controller.Create(GetNewUsoanestesico());

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
            var result = controller.Edit(1, GetTargetUsoanestesicoModel());

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
            var usoanestesicoModel = GetTargetUsoanestesicoModel(); // Obtém o modelo para deletar

            // Act
            var result = controller.Delete(1, usoanestesicoModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        // Métodos auxiliares
        private UsoanestesicoViewModel GetNewUsoanestesico()
        {
            return new UsoanestesicoViewModel
            {
                Id = 4,
                Quantidade = 15,
                Procedimento = "Procedimento Novo",
                Data = DateTime.Now,
                Cepa = "Cepa Nova",
                NumeroAnimais = 5
            };
        }

        private Usoanestesico GetTargetUsoanestesico()
        {
            return new Usoanestesico
            {
                Id = 1,
                Quantidade = 10,
                Procedimento = "Procedimento 1",
                Data = DateTime.Now.AddDays(-1),
                Cepa = "Cepa 1",
                NumeroAnimais = 2,
                IdPesquisador = 1,
                IdExperimento = 1,
                IdEntrada = 1,
                IdAnestesico = 1
            };
        }

        private UsoanestesicoViewModel GetTargetUsoanestesicoModel()
        {
            return new UsoanestesicoViewModel
            {
                Id = 1,
                Quantidade = 10,
                Procedimento = "Procedimento 1",
                Data = DateTime.Now.AddDays(-1),
                Cepa = "Cepa 1",
                NumeroAnimais = 2
            };
        }

        private IEnumerable<Usoanestesico> GetTestUsoanestesicos()
        {
            return new List<Usoanestesico>
            {
                new Usoanestesico
                {
                    Id = 1,
                    Quantidade = 10,
                    Procedimento = "Procedimento 1",
                    Data = DateTime.Now.AddDays(-1),
                    Cepa = "Cepa 1",
                    NumeroAnimais = 2
                },
                new Usoanestesico
                {
                    Id = 2,
                    Quantidade = 20,
                    Procedimento = "Procedimento 2",
                    Data = DateTime.Now.AddDays(-2),
                    Cepa = "Cepa 2",
                    NumeroAnimais = 3
                },
                new Usoanestesico
                {
                    Id = 3,
                    Quantidade = 30,
                    Procedimento = "Procedimento 3",
                    Data = DateTime.Now.AddDays(-3),
                    Cepa = "Cepa 3",
                    NumeroAnimais = 4
                }
            };
        }
    }
}
