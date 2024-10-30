using BiotLabWeb.Controllers;
using BiotLabWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Core;

namespace BiotLabWeb.Tests.Controllers
{
    // Implementação fictícia de IObituarioService
    public class MockObituarioService : IObituarioService
    {
        private readonly List<Obituario> _obituarios = new();

        public uint Create(Obituario obituario)
        {
            obituario.Id = (uint)(_obituarios.Count + 1); // Simula auto incremento
            _obituarios.Add(obituario);
            return obituario.Id;
        }

        public Obituario? Buscar(uint id)
        {
            return _obituarios.FirstOrDefault(o => o.Id == id);
        }

        public void Delete(uint id)
        {
            var obituario = _obituarios.FirstOrDefault(o => o.Id == id);
            if (obituario != null)
            {
                _obituarios.Remove(obituario);
            }
        }

        public IEnumerable<Obituario> GetAll() => _obituarios;
    }

    public class ObituarioControllerTests
    {
        private readonly ObituarioController _controller;
        private readonly MockObituarioService _obituarioService;

        public ObituarioControllerTests()
        {
            _obituarioService = new MockObituarioService();
            _controller = new ObituarioController(_obituarioService, null!); // Passando null para o mapper
        }

        public void Create_ValidObituario_ReturnsRedirectToIndex()
        {
            // Arrange
            var viewModel = new ObituarioViewModel
            {
                IdPesquisador = "1",
                Data = "01.000.000/2024-01",
                IdGaiola = "12345-678"
            };

            // Act
            var result = _controller.Create(viewModel) as RedirectToActionResult;

            // Assert
            if (result == null || result.ActionName != "Index")
            {
                throw new Exception("Teste falhou: o resultado não é um redirecionamento para Index.");
            }
        }

        public void Create_InvalidObituario_ReturnsView()
        {
            // Arrange
            var viewModel = new ObituarioViewModel
            {
                IdPesquisador = string.Empty, // Inválido
                Data = "data_invalida",
                IdGaiola = "123"
            };

            // Act
            var result = _controller.Create(viewModel) as ViewResult;

            // Assert
            if (result == null || result.ViewName != "Create" || _controller.ModelState.IsValid)
            {
                throw new Exception("Teste falhou: o resultado não é a View Create ou o ModelState é válido.");
            }
        }

        public void Index_ReturnsViewWithObituarios()
        {
            // Arrange
            _obituarioService.Create(new Obituario
            {
                IdPesquisador = 1,
                Data = DateTime.Now,
                IdGaiola = 101
            });

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            if (result == null || result.Model == null)
            {
                throw new Exception("Teste falhou: o resultado é nulo.");
            }

            var model = result.Model as IEnumerable<ObituarioViewModel>;
            if (model == null || !model.Any())
            {
                throw new Exception("Teste falhou: o modelo está vazio.");
            }
        }

        public void Details_ValidId_ReturnsViewWithObituario()
        {
            // Arrange
            var obituario = new Obituario
            {
                IdPesquisador = 1,
                Data = DateTime.Now,
                IdGaiola = 101
            };
            uint id = _obituarioService.Create(obituario);

            // Act
            var result = _controller.Details(id) as ViewResult;

            // Assert
            if (result == null)
            {
                throw new Exception("Teste falhou: o resultado é nulo.");
            }

            var model = result.Model as ObituarioViewModel;
            if (model == null || model.Id != id)
            {
                throw new Exception("Teste falhou: o modelo não corresponde ao ID esperado.");
            }
        }

        public void Delete_ValidId_RemovesObituario()
        {
            // Arrange
            var obituario = new Obituario
            {
                IdPesquisador = 1,
                Data = DateTime.Now,
                IdGaiola = 101
            };
            uint id = _obituarioService.Create(obituario);

            // Act
            _controller.Delete(id);

            // Assert
            var deletedObituario = _obituarioService.Buscar(id);
            if (deletedObituario != null)
            {
                throw new Exception("Teste falhou: o obituário não foi removido.");
            }
        }

        // Método para executar todos os testes
        public void RunAllTests()
        {
            Create_ValidObituario_ReturnsRedirectToIndex();
            Create_InvalidObituario_ReturnsView();
            Index_ReturnsViewWithObituarios();
            Details_ValidId_ReturnsViewWithObituario();
            Delete_ValidId_RemovesObituario();
        }
    }
}