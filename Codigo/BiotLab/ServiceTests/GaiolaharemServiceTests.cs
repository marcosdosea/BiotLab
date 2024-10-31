using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Tests
{
    [TestClass]
    public class GaiolaharemServiceTests
    {
        private BiotlabContext context;
        private IGaiolaharemService gaiolaharemService;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<BiotlabContext>()
                .UseInMemoryDatabase(databaseName: "BiotlabTestDB")
                .Options;

            context = new BiotlabContext(options);

            // Garante que o banco de dados está limpo antes de cada teste
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Popula o banco de dados com dados iniciais
            SeedDatabase();

            // Inicializa o serviço com o contexto
            gaiolaharemService = new GaiolaharemService(context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Limpa o banco de dados após cada teste
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        private void SeedDatabase()
        {
            // Adiciona Bioterio com todas as propriedades obrigatórias
            var bioterio = new Bioterio
            {
                Id = 1,
                Nome = "Biotério Central",
                Cep = "12345678",
                Email = "contato@bioterio.com",
                Estado = "SP",
                Telefone1 = "11999999999"
                // As propriedades opcionais podem ser deixadas em branco ou preenchidas se necessário
            };
            context.Bioterios.Add(bioterio);

            // Adiciona Gaiolas
            var gaiolas = new List<Gaiola>
            {
                new Gaiola
                {
                    Id = 1,
                    CodigoInterno = "Gaiola001",
                    NumeroMachos = 2,
                    NumeroFemeas = 3,
                    Localizacao = "Sala A",
                    Status = "N",
                    IdBioterio = 1
                },
                new Gaiola
                {
                    Id = 2,
                    CodigoInterno = "Gaiola002",
                    NumeroMachos = 4,
                    NumeroFemeas = 1,
                    Localizacao = "Sala B",
                    Status = "N",
                    IdBioterio = 1
                }
            };
            context.Gaiolas.AddRange(gaiolas);

            // Adiciona Harems
            var harems = new List<Harem>
            {
                new Harem
                {
                    Id = 1,
                    CodigoInterno = "Harem001",
                    NumeroMachos = 1,
                    NumeroFemeas = 2,
                    DataNascimento = DateTime.Now.AddMonths(-6),
                    Status = "A",
                    IdBioterio = 1
                },
                new Harem
                {
                    Id = 2,
                    CodigoInterno = "Harem002",
                    NumeroMachos = 2,
                    NumeroFemeas = 3,
                    DataNascimento = DateTime.Now.AddMonths(-3),
                    Status = "A",
                    IdBioterio = 1
                }
            };
            context.Harems.AddRange(harems);

            // Adiciona Pesquisadores com todas as propriedades obrigatórias
            var pesquisadores = new List<Pesquisador>
            {
                new Pesquisador
                {
                    Id = 1,
                    Cpf = "12345678901",
                    Nome = "Dr. Silva",
                    Cep = "12345678",
                    Estado = "SP",
                    Telefone1 = "11999999999",
                    Email = "dr.silva@example.com"
                    // As propriedades opcionais podem ser deixadas em branco ou preenchidas se necessário
                },
                new Pesquisador
                {
                    Id = 2,
                    Cpf = "09876543210",
                    Nome = "Dr. Souza",
                    Cep = "87654321",
                    Estado = "RJ",
                    Telefone1 = "21988888888",
                    Email = "dr.souza@example.com"
                }
            };
            context.Pesquisadors.AddRange(pesquisadores);

            // Adiciona GaiolaHarem
            var gaiolaHarems = new List<Gaiolaharem>
            {
                new Gaiolaharem
                {
                    IdGaiola = 1,
                    IdHarem = 1,
                    DataPovoamento = DateTime.Now.AddDays(-10),
                    IdPesquisador = 1
                },
                new Gaiolaharem
                {
                    IdGaiola = 2,
                    IdHarem = 2,
                    DataPovoamento = DateTime.Now.AddDays(-5),
                    IdPesquisador = 2
                }
            };
            context.Gaiolaharems.AddRange(gaiolaHarems);

            // Salva as mudanças no banco de dados
            context.SaveChanges();
        }

        [TestMethod]
        public void CreateTest()
        {
            // Arrange
            var newGaiolaharem = new Gaiolaharem
            {
                IdGaiola = 1,
                IdHarem = 2,
                DataPovoamento = DateTime.Now,
                IdPesquisador = 1
            };

            // Act
            gaiolaharemService.Create(newGaiolaharem);

            // Assert
            var createdGaiolaharem = gaiolaharemService.Get(1, 2);
            Assert.IsNotNull(createdGaiolaharem);
            Assert.AreEqual(1U, createdGaiolaharem.IdGaiola);
            Assert.AreEqual(2U, createdGaiolaharem.IdHarem);
            Assert.AreEqual(1U, createdGaiolaharem.IdPesquisador);
        }

        [TestMethod]
        public void GetTest()
        {
            // Act
            var gaiolaharem = gaiolaharemService.Get(1, 1);

            // Assert
            Assert.IsNotNull(gaiolaharem);
            Assert.AreEqual(1U, gaiolaharem.IdGaiola);
            Assert.AreEqual(1U, gaiolaharem.IdHarem);
            Assert.AreEqual(1U, gaiolaharem.IdPesquisador);
        }

        [TestMethod]
        public void GetAllTest()
        {
            // Act
            var gaiolaharems = gaiolaharemService.GetAll();

            // Assert
            Assert.IsNotNull(gaiolaharems);
            Assert.AreEqual(2, gaiolaharems.Count());
        }

        [TestMethod]
        public void UpdateTest()
        {
            // Arrange
            var existingGaiolaharem = gaiolaharemService.Get(1, 1);
            existingGaiolaharem.DataPovoamento = DateTime.Now.AddDays(-2);
            existingGaiolaharem.IdPesquisador = 2;

            // Act
            gaiolaharemService.Update(existingGaiolaharem);

            // Assert
            var updatedGaiolaharem = gaiolaharemService.Get(1, 1);
            Assert.IsNotNull(updatedGaiolaharem);
            Assert.AreEqual(2U, updatedGaiolaharem.IdPesquisador);
            Assert.AreEqual(DateTime.Now.AddDays(-2).Date, updatedGaiolaharem.DataPovoamento.Date);
        }

        [TestMethod]
        public void DeleteTest()
        {
            // Act
            gaiolaharemService.Delete(2, 2);

            // Assert
            var deletedGaiolaharem = gaiolaharemService.Get(2, 2);
            Assert.IsNull(deletedGaiolaharem);
        }
    }
}
