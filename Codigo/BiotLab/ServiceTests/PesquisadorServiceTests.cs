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
    public class PesquisadorServiceTests
    {
        private BiotlabContext context;
        private IPesquisadorService pesquisadorService;

        [TestInitialize]
        public void Initialize()
        {
            // Configurando o banco de dados em memória para testes
            var builder = new DbContextOptionsBuilder<BiotlabContext>();
            builder.UseInMemoryDatabase("BiotlabPesquisadores");
            var options = builder.Options;

            context = new BiotlabContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Criação de uma lista de pesquisadores
            var pesquisadores = new List<Pesquisador>
            {
                new()
                {
                    Id = 1,
                    Cpf = "12345678901",
                    Nome = "Pesquisador 1",
                    Cep = "12345-678",
                    Estado = "SP",
                    Telefone1 = "123456789",
                    Email = "pesquisador1@example.com"
                },
                new()
                {
                    Id = 2,
                    Cpf = "10987654321",
                    Nome = "Pesquisador 2",
                    Cep = "87654-321",
                    Estado = "RJ",
                    Telefone1 = "987654321",
                    Email = "pesquisador2@example.com"
                }
            };

            context.AddRange(pesquisadores);
            context.SaveChanges();

            pesquisadorService = new PesquisadorService(context);
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
            // Criando um novo pesquisador
            var novoPesquisador = new Pesquisador
            {
                Cpf = "11122233344",
                Nome = "Pesquisador 3",
                Cep = "11223-334",
                Estado = "MG",
                Telefone1 = "555555555",
                Email = "pesquisador3@example.com"
            };

            var createdId = pesquisadorService.Create(novoPesquisador);

            // Verificando a criação
            Assert.AreEqual(3, pesquisadorService.GetAll().Count(), "O número de pesquisadores deve ser 3.");
            var pesquisador = pesquisadorService.Buscar(createdId);
            Assert.IsNotNull(pesquisador, "O pesquisador criado deve existir.");
            Assert.AreEqual("Pesquisador 3", pesquisador.Nome, "O nome do pesquisador não corresponde.");
        }

        [TestMethod]
        public void DeleteTest()
        {
            // Verificando a contagem antes da deleção
            var initialCount = pesquisadorService.GetAll().Count();
            Assert.AreEqual(2, initialCount, "A contagem inicial de pesquisadores está errada.");

            // Deletando o primeiro pesquisador
            pesquisadorService.Delete(1);

            // Verificando se foi removido
            var postDeleteCount = pesquisadorService.GetAll().Count();
            Assert.AreEqual(1, postDeleteCount, "O número de pesquisadores após a deleção está errado.");

            var pesquisador = pesquisadorService.Buscar(1);
            Assert.IsNull(pesquisador, "O pesquisador ainda existe após a deleção.");
        }

        [TestMethod]
        public void GetTest()
        {
            // Buscando um pesquisador pelo ID
            var pesquisador = pesquisadorService.Buscar(1);
            Assert.IsNotNull(pesquisador, "O pesquisador deveria ser encontrado.");
            Assert.AreEqual("Pesquisador 1", pesquisador.Nome, "O nome do pesquisador não corresponde.");
        }

        [TestMethod]
        public void GetAllTest()
        {
            // Obtendo todos os pesquisadores
            var listaPesquisadores = pesquisadorService.GetAll();

            // Verificando se a lista contém todos os pesquisadores esperados
            Assert.IsInstanceOfType(listaPesquisadores, typeof(IEnumerable<Pesquisador>), "O resultado não é uma lista de pesquisadores.");
            Assert.IsNotNull(listaPesquisadores, "A lista de pesquisadores não deve ser nula.");
            Assert.AreEqual(2, listaPesquisadores.Count(), "O número de pesquisadores deve ser 2.");
        }
    }
}
