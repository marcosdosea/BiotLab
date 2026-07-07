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
    public class UsoanestesicoServiceTests
    {
        private BiotlabContext context = null!;
        private IUsoanestesicoService usoanestesicoService = null!;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<BiotlabContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new BiotlabContext(options);
            usoanestesicoService = new UsoanestesicoService(context);
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            context.Instituicaos.Add(new Instituicao
            {
                Id = 1,
                Nome = "Instituicao Teste",
                Cnpj = "12345678901234",
                Cep = "12345678",
                Cidade = "Cidade A",
                Estado = "SP",
                Telefone1 = "11111111",
                Email = "instituicao@exemplo.com"
            });

            context.Pesquisadors.AddRange(
                new Pesquisador { Id = 1, Nome = "Pesquisador 1", Cpf = "12345678901", Cep = "12345678", Estado = "SP", Telefone1 = "11111111", Email = "p1@exemplo.com" },
                new Pesquisador { Id = 2, Nome = "Pesquisador 2", Cpf = "12345678902", Cep = "12345678", Estado = "SP", Telefone1 = "22222222", Email = "p2@exemplo.com" },
                new Pesquisador { Id = 3, Nome = "Pesquisador 3", Cpf = "12345678903", Cep = "12345678", Estado = "SP", Telefone1 = "33333333", Email = "p3@exemplo.com" });

            context.Experimentos.AddRange(
                new Experimento { Id = 1, Titulo = "Projeto A", Cepa = "Cepa A", DataInicio = new DateTime(2024, 1, 1), DataFim = new DateTime(2024, 1, 10), IdPesquisador = 1 },
                new Experimento { Id = 2, Titulo = "Projeto B", Cepa = "Cepa B", DataInicio = new DateTime(2024, 1, 1), DataFim = new DateTime(2024, 1, 10), IdPesquisador = 2 },
                new Experimento { Id = 3, Titulo = "Projeto C", Cepa = "Cepa C", DataInicio = new DateTime(2024, 1, 1), DataFim = new DateTime(2024, 1, 10), IdPesquisador = 3 });

            context.Fornecedors.Add(new Fornecedor
            {
                Id = 1,
                Nome = "Fornecedor 1",
                Cnpj = "12345678901234",
                Cep = "12345678",
                Cidade = "Cidade A",
                Estado = "SP",
                Telefone1 = "11111111",
                Email = "fornecedor@exemplo.com",
                IdInstituicao = 1
            });

            context.Anestesicos.AddRange(
                new Anestesico { Id = 1, Nome = "Anestesico 1", Marca = "Marca A", Concentracao = 1, IdInstituicao = 1 },
                new Anestesico { Id = 2, Nome = "Anestesico 2", Marca = "Marca B", Concentracao = 2, IdInstituicao = 1 },
                new Anestesico { Id = 3, Nome = "Anestesico 3", Marca = "Marca C", Concentracao = 3, IdInstituicao = 1 });

            context.Entrada.AddRange(
                new Entradum { Id = 1, DataEntrada = new DateTime(2024, 1, 1), NumeroNotaFiscal = "NF001", IdFornecedor = 1, IdInstituicao = 1 },
                new Entradum { Id = 2, DataEntrada = new DateTime(2024, 1, 2), NumeroNotaFiscal = "NF002", IdFornecedor = 1, IdInstituicao = 1 },
                new Entradum { Id = 3, DataEntrada = new DateTime(2024, 1, 3), NumeroNotaFiscal = "NF003", IdFornecedor = 1, IdInstituicao = 1 });

            context.Entradaanestesicos.AddRange(
                new Entradaanestesico { IdEntrada = 1, IdAnestesico = 1, Lote = "Lote 1", Quantidade = 10, ValorUnitario = 1, SubTotal = 10 },
                new Entradaanestesico { IdEntrada = 2, IdAnestesico = 2, Lote = "Lote 2", Quantidade = 10, ValorUnitario = 1, SubTotal = 10 },
                new Entradaanestesico { IdEntrada = 3, IdAnestesico = 3, Lote = "Lote 3", Quantidade = 10, ValorUnitario = 1, SubTotal = 10 });

            context.SaveChanges();

            var usosanestesicos = new List<Usoanestesico>
            {
                new()
                {
                    Id = 1,
                    Quantidade = 10,
                    Procedimento = "Procedimento 1",
                    Data = new DateTime(2024, 1, 2),
                    Cepa = "Cepa A",
                    NumeroAnimais = 5,
                    IdPesquisador = 1,
                    IdExperimento = 1,
                    IdEntrada = 1,
                    IdAnestesico = 1
                },
                new()
                {
                    Id = 2,
                    Quantidade = 20,
                    Procedimento = "Procedimento 2",
                    Data = new DateTime(2024, 1, 1),
                    Cepa = "Cepa B",
                    NumeroAnimais = 10,
                    IdPesquisador = 2,
                    IdExperimento = 2,
                    IdEntrada = 2,
                    IdAnestesico = 2
                }
            };

            foreach (var usoanestesico in usosanestesicos)
            {
                usoanestesicoService.Create(usoanestesico);
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            var novoUsoanestesico = new Usoanestesico
            {
                Quantidade = 30,
                Procedimento = "Procedimento 3",
                Data = new DateTime(2024, 1, 3),
                Cepa = "Cepa C",
                NumeroAnimais = 15,
                IdPesquisador = 3,
                IdExperimento = 3,
                IdEntrada = 3,
                IdAnestesico = 3
            };

            var createdId = usoanestesicoService.Create(novoUsoanestesico);

            Assert.AreEqual(3, usoanestesicoService.GetAll().Count());
            var usoanestesico = usoanestesicoService.Get(createdId);
            Assert.IsNotNull(usoanestesico);
            Assert.AreEqual("Procedimento 3", usoanestesico.Procedimento);
            Assert.AreEqual(30, usoanestesico.Quantidade);
        }

        [TestMethod]
        public void DeleteTest()
        {
            usoanestesicoService.Delete(1);

            Assert.AreEqual(1, usoanestesicoService.GetAll().Count());
            var usoanestesico = usoanestesicoService.Get(1);
            Assert.IsNull(usoanestesico);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var usoAnestesico = usoanestesicoService.Get(2);
            Assert.IsNotNull(usoAnestesico);

            usoAnestesico.Procedimento = "Procedimento Alterado";
            usoAnestesico.Quantidade = 25;
            usoanestesicoService.Update(usoAnestesico);

            usoAnestesico = usoanestesicoService.Get(2);
            Assert.IsNotNull(usoAnestesico);
            Assert.AreEqual("Procedimento Alterado", usoAnestesico.Procedimento);
            Assert.AreEqual(25, usoAnestesico.Quantidade);
        }

        [TestMethod]
        public void GetTest()
        {
            var usoanestesico = usoanestesicoService.Get(1);
            Assert.IsNotNull(usoanestesico);
            Assert.AreEqual("Procedimento 1", usoanestesico.Procedimento);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var listaUsosanestesicos = usoanestesicoService.GetAll();

            Assert.IsInstanceOfType(listaUsosanestesicos, typeof(IEnumerable<Usoanestesico>));
            Assert.IsNotNull(listaUsosanestesicos);
            Assert.AreEqual(2, listaUsosanestesicos.Count());
            Assert.AreEqual(1u, listaUsosanestesicos.First().Id);
            Assert.AreEqual("Procedimento 1", listaUsosanestesicos.First().Procedimento);
        }
    }
}
