using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Microsoft.EntityFrameworkCore;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Service;

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
            var builder = new DbContextOptionsBuilder<BiotlabContext>();
            builder.UseInMemoryDatabase("BiotlabTestDB");
            var options = builder.Options;

            context = new BiotlabContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Criando dados de teste para as entidades relacionadas
            var gaiolas = new List<Gaiola>
            {
                new Gaiola { Id = 1, CodigoInterno = "Gaiola001", NumeroMachos = 5, NumeroFemeas = 5, Localizacao = "Sala A", Status = "N", IdBioterio = 1 },
                new Gaiola { Id = 2, CodigoInterno = "Gaiola002", NumeroMachos = 3, NumeroFemeas = 7, Localizacao = "Sala B", Status = "N", IdBioterio = 1 }
            };

            var harems = new List<Harem>
            {
                new Harem { Id = 1, CodigoInterno = "Harem Alpha" },
                new Harem { Id = 2, CodigoInterno = "Harem Beta" }
            };

            var pesquisadores = new List<Pesquisador>
            {
                new Pesquisador { Id = 1, Nome = "Dr. Silva" },
                new Pesquisador { Id = 2, Nome = "Dr. Souza" }
            };

            context.Gaiolas.AddRange(gaiolas);
            context.Harems.AddRange(harems);
            context.Pesquisadors.AddRange(pesquisadores);

            // Dados de teste para Gaiolaharem
            var gaiolaharems = new List<Gaiolaharem>
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

            context.Gaiolaharems.AddRange(gaiolaharems);
            context.SaveChanges();

            gaiolaharemService = new GaiolaharemService(context);
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

        [TestMethod]
        public void GetTest()
        {
            // Act
            var gaiolaharem = gaiolaharemService.Get(1, 1);

            // Assert
            Assert.IsNotNull(gaiolaharem);
            Assert.AreEqual(1U, gaiolaharem.IdGaiola);
            Assert.AreEqual(1U, gaiolaharem.IdHarem);
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
    }
}
