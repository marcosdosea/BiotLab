using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Tests
{
    [TestClass]
    public class ObituarioServiceTests
    {
        private BiotlabContext context;
        private IObituarioService obituarioService;

        [TestInitialize]
        public void Initialize()
        {
            // Configurando o banco de dados em memória para testes
            var builder = new DbContextOptionsBuilder<BiotlabContext>();
            builder.UseInMemoryDatabase("BiotlabObituarios");
            var options = builder.Options;

            context = new BiotlabContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Criação de uma lista de obituários
            var obituarios = new List<Obituario>
            {
                new()
                {
                    Id = 1,
                    Data = new DateTime(2025, 8, 21, 9, 0, 0),
                    IdGaiola = 101,
                    IdPesquisador = 202
                },
                new()
                {
                    Id = 2,
                    Data = new DateTime(2025, 9, 20, 18, 30, 0),
                    IdGaiola = 102,
                    IdPesquisador = 203
                },
                new()
                {
                    Id = 3,
                    Data = new DateTime(2025, 9, 21),
                    IdGaiola = 103,
                    IdPesquisador = 204
                }
            };

            context.AddRange(obituarios);
            context.SaveChanges();

            obituarioService = new ObituarioService(context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [TestMethod]
        public void CreateTest()
        {
            // Criando um novo obituário
            var novoObituario = new Obituario
            {
                Data = DateTime.Now.AddDays(-2),
                IdGaiola = 104,
                IdPesquisador = 205
            };

            var createdId = obituarioService.Create(novoObituario);

            // Verificando a criação
            Assert.AreEqual(4, obituarioService.GetAll().Count());
            var obituario = obituarioService.Buscar(createdId);
            Assert.IsNotNull(obituario);
            Assert.AreEqual(104u, obituario.IdGaiola); // Especificando uint
        }

        [TestMethod]
        public void DeleteTest()
        {
            // Deletando o primeiro obituário
            obituarioService.Delete(1);

            // Verificando se foi removido
            Assert.AreEqual(2, obituarioService.GetAll().Count());
            var obituario = obituarioService.Buscar(1);
            Assert.IsNull(obituario);
        }

        [TestMethod]
        public void GetTest()
        {
            // Buscando um obituário pelo ID
            var obituario = obituarioService.Buscar(1);
            Assert.IsNotNull(obituario);
            Assert.AreEqual(101u, obituario.IdGaiola); // Especificando uint
        }

        [TestMethod]
        public void GetAllTest()
        {
            // Obtendo todos os obituários
            var listaObituarios = obituarioService.GetAll();

            // Verificando se a lista contém todos os obituários esperados
            Assert.IsInstanceOfType(listaObituarios, typeof(IEnumerable<Obituario>));
            Assert.IsNotNull(listaObituarios);
            Assert.AreEqual(3, listaObituarios.Count());
            Assert.AreEqual(3u, listaObituarios.First().Id); // Especificando uint
            Assert.AreEqual(103u, listaObituarios.First().IdGaiola); // Especificando uint
        }

        [TestMethod]
        public void GetByPeriodoTest_IncluiDataInicialEFinal()
        {
            var listaObituarios = obituarioService
                .GetByPeriodo(new DateTime(2025, 8, 21), new DateTime(2025, 9, 20))
                .ToList();

            Assert.AreEqual(2, listaObituarios.Count);
            Assert.IsTrue(listaObituarios.Any(x => x.Id == 1));
            Assert.IsTrue(listaObituarios.Any(x => x.Id == 2));
            Assert.IsFalse(listaObituarios.Any(x => x.Id == 3));
        }
    }
}
