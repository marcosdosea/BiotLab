using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Tests
{
    [TestClass()]
    public class EntradumServiceTests
    {
        private BiotlabContext context = null!;
        private IEntradumService entradumService = null!;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<BiotlabContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var options = builder.Options;

            context = new BiotlabContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            SeedDatabase(); // Método auxiliar para popular o banco
            entradumService = new EntradumService(context);
        }

        private void SeedDatabase()
        {
            var instituicao = InstituicaoMock();
            var fornecedor = FornecedorMock();
            context.Instituicaos.Add(instituicao);
            context.Fornecedors.Add(fornecedor);
            var entradas = new List<Entradum>
            {
                new Entradum
                {
                    Id = 1,
                    DataEntrada = DateTime.MinValue,
                    IdFornecedorNavigation = fornecedor,
                    IdInstituicaoNavigation = instituicao,
                    NumeroNotaFiscal = "1"
                },
                new Entradum
                {
                    Id = 2,
                    DataEntrada = DateTime.MaxValue,
                    IdFornecedorNavigation = fornecedor,
                    IdInstituicaoNavigation = instituicao,
                    NumeroNotaFiscal = "2"
                },
                new Entradum
                {
                    Id = 3,
                    DataEntrada = DateTime.MinValue,
                    IdFornecedorNavigation = fornecedor,
                    IdInstituicaoNavigation = instituicao,
                    NumeroNotaFiscal = "3"
                }
            };

            context.AddRange(entradas);
            context.SaveChanges();
        }


        public Fornecedor FornecedorMock()
        {
            return new Fornecedor
            {
                Id = 1,
                Nome = "Fornecedor Exemplo",
                Cnpj = "12345678000190",
                Cep = "12345000",
                Rua = "Rua Exemplo",
                Bairro = "Bairro Exemplo",
                Cidade = "Cidade Exemplo",
                Numero = "123",
                Complemento = "Apto 1",
                Estado = "SP",
                Telefone1 = "(11) 1234-5678",
                Telefone2 = "(11) 8765-4321",
                Email = "fornecedor@exemplo.com",
                IdInstituicao = 1,
            };
        }

        private Instituicao InstituicaoMock()
        {
            return new Instituicao
            {
                Id = 1,
                Nome = "Fornecedor Exemplo",
                Cnpj = "12345678000190",
                Cep = "12345000",
                Rua = "Rua Exemplo",
                Bairro = "Bairro Exemplo",
                Cidade = "Cidade Exemplo",
                Numero = "123",
                Complemento = "Apto 1",
                Estado = "SP",
                Email = "fornecedor@exemplo.com",
                Telefone1 = "(11) 1234-5678",
                Telefone2 = "(11) 8765-4321",
            };
        }


        [TestMethod()]
        public void CreateTest()
        {
            var entrada = new Entradum
            {
                DataEntrada = DateTime.Parse("22/10/2003"),
                IdFornecedor = 1,
                IdInstituicao = 1,
                NumeroNotaFiscal = "4",
            };

            var id = entradumService.Create(entrada);
            Assert.AreEqual(id, 4u);
            Assert.AreEqual(entradumService.GetAll().Count(), 4);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            entradumService.Delete(3);
            Assert.AreEqual(entradumService.GetAll().Count(), 2);
            Assert.IsNull(entradumService.Get(3));
        }

        [TestMethod()]
        public void GetTest()
        {
            var entrada = entradumService.Get(1);
            Assert.IsNotNull(entrada);
            Assert.AreEqual(entrada.Id, 1u);
            Assert.AreEqual(entrada.NumeroNotaFiscal, "1");
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var entradas = entradumService.GetAll().ToList();
            Assert.AreEqual (entradas.Count, 3);
            Assert.AreEqual(entradas.First().Id, 1u);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            var entrada = new Entradum
            {
                Id = 1,
                DataEntrada = DateTime.Parse("22/12/2002"),
                NumeroNotaFiscal = "5",
                IdFornecedor = 1,
                IdInstituicao = 1,
            };

            entradumService.Update(entrada);
            var entradaAtualizada = entradumService.Get(1);
            Assert.IsNotNull(entradaAtualizada);
            Assert.AreEqual(entradaAtualizada.NumeroNotaFiscal, "5");
            Assert.AreEqual(entradaAtualizada.DataEntrada, DateTime.Parse("22/12/2002"));
        }
    }
}