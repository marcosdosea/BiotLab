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
    public class EntradaanestesicoServiceTests
    {
        private BiotlabContext context;
        private IEntradaanestesicoService EntradasnestesicoService;

        [TestInitialize]
        public void Initialize()
        {
            // Configurando o banco de dados em memória para testes
            var builder = new DbContextOptionsBuilder<BiotlabContext>();
            builder.UseInMemoryDatabase("BiotlabTestDB");
            var options = builder.Options;

            context = new BiotlabContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Criando entidades relacionadas
            var anestesicos = new List<Anestesico>
            {
            new Anestesico { Id = 1, Nome = "Anestésico A", Marca = "Marca A" },
            new Anestesico { Id = 2, Nome = "Anestésico B", Marca = "Marca B" },
            new Anestesico { Id = 3, Nome = "Anestésico C", Marca = "Marca C" }
            };


            var entradas = new List<Entradum>
        {
             new Entradum
             {
            Id = 1,
            DataEntrada = DateTime.Now.AddDays(-1),
            NumeroNotaFiscal = "NF001",
            // Defina outras propriedades obrigatórias, se houver
            },
            new Entradum
            {
             Id = 2,
             DataEntrada = DateTime.Now,
             NumeroNotaFiscal = "NF002",
        // Defina outras propriedades obrigatórias, se houver
             }
         };

        

            context.Anestesicos.AddRange(anestesicos);
            context.Entrada.AddRange(entradas);

            // Criando uma lista de entradas de anestésicos
            var entradaAnestesicos = new List<Entradaanestesico>
            {
                new Entradaanestesico
                {
                    IdEntrada = 1,
                    IdAnestesico = 1,
                    Quantidade = 10,
                    Lote = "Lote001",
                    ValorUnitario = 50,
                    SubTotal = 500
                },
                new Entradaanestesico
                {
                    IdEntrada = 1,
                    IdAnestesico = 2,
                    Quantidade = 5,
                    Lote = "Lote002",
                    ValorUnitario = 100,
                    SubTotal = 500
                }
            };

            context.Entradaanestesicos.AddRange(entradaAnestesicos);
            context.SaveChanges();

            EntradasnestesicoService = new EntradaanestesicoService(context);
        }

        [TestMethod]
        public void CreateTest()
        {
            // Criando uma nova entrada de anestésico
            var novaEntradaAnestesico = new Entradaanestesico
            {
                IdEntrada = 2,
                IdAnestesico = 3,
                Quantidade = 15,
                Lote = "Lote003",
                ValorUnitario = 200,
                SubTotal = 3000
            };

            EntradasnestesicoService.Create(novaEntradaAnestesico);

            // Verificando a criação
            var allEntries = EntradasnestesicoService.GetAll();
            Assert.AreEqual(3, allEntries.Count());

            var entradaAnestesico = EntradasnestesicoService.Get(2, 3);
            Assert.IsNotNull(entradaAnestesico);
            Assert.AreEqual("Lote003", entradaAnestesico.Lote);
            Assert.AreEqual(3000, entradaAnestesico.SubTotal);
        }

        [TestMethod]
        public void DeleteTest()
        {
            // Deletando uma entrada de anestésico existente
            EntradasnestesicoService.Delete(1, 1);

            // Verificando se foi removida
            var entradaAnestesico = EntradasnestesicoService.Get(1, 1);
            Assert.IsNull(entradaAnestesico);
        }

        [TestMethod]
        public void UpdateTest()
        {
            // Atualizando uma entrada de anestésico existente
            var entradaAnestesico = EntradasnestesicoService.Get(1, 2);
            entradaAnestesico.Quantidade = 20;
            entradaAnestesico.SubTotal = entradaAnestesico.Quantidade * entradaAnestesico.ValorUnitario;
            EntradasnestesicoService.Update(entradaAnestesico);

            // Verificando se a atualização foi bem-sucedida
            var updatedEntradaAnestesico = EntradasnestesicoService.Get(1, 2);
            Assert.IsNotNull(updatedEntradaAnestesico);
            Assert.AreEqual(20, updatedEntradaAnestesico.Quantidade);
            Assert.AreEqual(2000, updatedEntradaAnestesico.SubTotal);
        }

        [TestMethod]
        public void GetTest()
        {
            // Buscando uma entrada de anestésico pela chave composta
            var entradaAnestesico = EntradasnestesicoService.Get(1, 1);
            Assert.IsNotNull(entradaAnestesico);
            Assert.AreEqual("Lote001", entradaAnestesico.Lote);
        }

        [TestMethod]
        public void GetAllTest()
        {
            // Obtendo todas as entradas de anestésicos
            var listaEntradaAnestesicos = EntradasnestesicoService.GetAll();

            // Verificando se a lista contém todas as entradas esperadas
            Assert.IsInstanceOfType(listaEntradaAnestesicos, typeof(IEnumerable<Entradaanestesico>));
            Assert.IsNotNull(listaEntradaAnestesicos);
            Assert.AreEqual(2, listaEntradaAnestesicos.Count());
        }
    }
}
