using AutoMapper;
using BiotLabWeb.Controllers;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BiotLabWeb.Controllers.Tests
{
    [TestClass]
    public class PesquisadorControllerTests
    {
        private PesquisadorController _controller;
        private IPesquisadorService _pesquisadorService;
        private IMapper _mapper;
        private List<Pesquisador> _pesquisadorList;

        [TestInitialize]
        public void Setup()
        {
            // Configuração do AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Pesquisador, PesquisadorViewModel>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            // Inicialização de uma lista em memória para simular o serviço
            _pesquisadorList = new List<Pesquisador>();
            _pesquisadorService = new PesquisadorServiceInMemory(_pesquisadorList);

            _controller = new PesquisadorController(_pesquisadorService, _mapper);
        }

        [TestMethod]
        public void Create_Post_ReturnsRedirectAndAddsPesquisador()
        {
            // Arrange
            var pesquisador = new PesquisadorViewModel { Nome = "Novo Pesquisador", Email = "novo@example.com" };

            // Act
            var result = _controller.Create(pesquisador) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var createdPesquisador = _pesquisadorList.FirstOrDefault(p => p.Email == "novo@example.com");
            Assert.IsNotNull(createdPesquisador);
            Assert.AreEqual("Novo Pesquisador", createdPesquisador.Nome);
        }

        [TestMethod]
        public void Index_ReturnsViewResult_WithListOfPesquisadores()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as IEnumerable<PesquisadorViewModel>;
            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void Details_ReturnsViewResult_WithPesquisador()
        {
            // Arrange
            var pesquisador = new Pesquisador { Id = 1, Nome = "Pesquisador1", Email = "email1@example.com" };
            _pesquisadorService.Create(pesquisador);

            // Act
            var result = _controller.Details(1);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as PesquisadorViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual("Pesquisador1", model.Nome);
        }

        [TestMethod]
        public void Edit_Post_ReturnsRedirectAndUpdatesPesquisador()
        {
            // Arrange
            var pesquisador = new Pesquisador { Id = 1, Nome = "Pesquisador Atualizado", Email = "atualizado@example.com" };
            _pesquisadorService.Create(pesquisador);
            var pesquisadorViewModel = new PesquisadorViewModel { Id = 1, Nome = "Pesquisador Atualizado", Email = "atualizado@example.com" };

            // Act
            var result = _controller.Edit(1, pesquisadorViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var updatedPesquisador = _pesquisadorList.FirstOrDefault(p => p.Email == "atualizado@example.com");
            Assert.IsNotNull(updatedPesquisador);
            Assert.AreEqual("Pesquisador Atualizado", updatedPesquisador.Nome);
        }

        [TestMethod]
        public void Delete_Post_ReturnsRedirectAndDeletesPesquisador()
        {
            // Arrange
            var pesquisador = new Pesquisador { Id = 1, Nome = "Pesquisador1", Email = "email1@example.com" };
            _pesquisadorService.Create(pesquisador);
            var pesquisadorViewModel = new PesquisadorViewModel { Id = 1, Nome = "Pesquisador1", Email = "email1@example.com" };

            // Act
            var result = _controller.Delete(1, pesquisadorViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var deletedPesquisador = _pesquisadorList.FirstOrDefault(p => p.Id == 1);
            Assert.IsNull(deletedPesquisador);
        }

    }

    public class PesquisadorServiceInMemory : IPesquisadorService
    {
        private readonly List<Pesquisador> _pesquisadorList;

        public PesquisadorServiceInMemory(List<Pesquisador> pesquisadorList)
        {
            _pesquisadorList = pesquisadorList;
        }

        public uint Create(Pesquisador pesquisador)
        {
            pesquisador.Id = (uint)(_pesquisadorList.Count + 1);
            _pesquisadorList.Add(pesquisador);
            return pesquisador.Id;
        }

        public void Delete(uint id)
        {
            var pesquisador = _pesquisadorList.FirstOrDefault(p => p.Id == id);
            if (pesquisador != null)
            {
                _pesquisadorList.Remove(pesquisador);
            }
        }

        public Pesquisador? Buscar(uint id)
        {
            return _pesquisadorList.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Pesquisador> GetAll()
        {
            return _pesquisadorList;
        }

        public void Update(Pesquisador pesquisador)
        {
            var existingPesquisador = _pesquisadorList.FirstOrDefault(p => p.Id == pesquisador.Id);
            if (existingPesquisador != null)
            {
                existingPesquisador.Nome = pesquisador.Nome;
                existingPesquisador.Email = pesquisador.Email;
                existingPesquisador.Cpf = pesquisador.Cpf;
                existingPesquisador.Cep = pesquisador.Cep;
                existingPesquisador.Rua = pesquisador.Rua;
                existingPesquisador.Bairro = pesquisador.Bairro;
                existingPesquisador.Cidade = pesquisador.Cidade;
                existingPesquisador.Numero = pesquisador.Numero;
                existingPesquisador.Complemento = pesquisador.Complemento;
                existingPesquisador.Estado = pesquisador.Estado;
                existingPesquisador.Telefone1 = pesquisador.Telefone1;
                existingPesquisador.Telefone2 = pesquisador.Telefone2;
            }
        }
    }
}
