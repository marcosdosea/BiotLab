using Core;
using Core.Service;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;

namespace ServiceTests
{
    [TestClass]
    public class ObituarioServiceTests
    {
        private ObituarioService? _service;
        private BiotlabContext? _context;

        [TestInitialize]
        public void Setup()
        {
            _context = new BiotlabContext();
            _service = new ObituarioService(_context);
        }

        [TestMethod]
        public void Create_Valido_RetornaId()
        {
            // Arrange
            var obituario = new Obituario
            {
                Id = 1,
                Data = DateTime.Now,
                IdGaiola = 101,
                IdPesquisador = 202
            };

            // Act
            uint idResultado = _service!.Create(obituario);

            // Assert
            Assert.IsTrue(idResultado > 0);
        }

        [TestMethod]
        public void Buscar_Existente_RetornaObituario()
        {
            // Arrange
            var obituario = new Obituario
            {
                Data = DateTime.Now,
                IdGaiola = 101,
                IdPesquisador = 202
            };
            uint id = _service!.Create(obituario);

            // Act
            var resultado = _service.Buscar(id);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(id, resultado?.Id);
        }

        [TestMethod]
        public void Buscar_Inexistente_RetornaNull()
        {
            // Act
            var resultado = _service!.Buscar(999);

            // Assert
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void Delete_Existente_RetornaTrue()
        {
            // Arrange
            var obituario = new Obituario
            {
                Data = DateTime.Now,
                IdGaiola = 101,
                IdPesquisador = 202
            };
            uint id = _service!.Create(obituario);

            // Act
            _service.Delete(id);

            // Assert
            var resultado = _service.Buscar(id);
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void Delete_Inexistente_NaoGeraErro()
        {
            // Act
            _service!.Delete(999);

            Assert.IsTrue(true);
        }
    }
}