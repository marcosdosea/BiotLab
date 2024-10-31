using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Tests
{
    [TestClass()]
    public class ExperimentoServiceTests : IDisposable
    {
        private BiotlabContext context;
        private IExperimentoService experimentoService;

        [TestInitialize]
        public void Initialize()
        {
            // Configurando o banco de dados em memória para testes
            var builder = new DbContextOptionsBuilder<BiotlabContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString()); // Usar um banco de dados exclusivo por teste
            var options = builder.Options;

            context = new BiotlabContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            SeedDatabase(); // Método auxiliar para popular o banco
            experimentoService = new ExperimentoService(context);
        }

        private void SeedDatabase()
        {
            // Criação de uma lista de experimentos
            var experimentos = new List<Experimento>
            {
                new Experimento
                {
                    DataInicio = DateTime.Now.AddDays(-10),
                    DataFim = DateTime.Now.AddDays(-5),
                    Cepa = "Cepa A",
                    IdPesquisador = 1,
                },
                new Experimento
                {
                    DataInicio = DateTime.Now.AddDays(-4),
                    DataFim = DateTime.Now,
                    Cepa = "Cepa B",
                    IdPesquisador = 2,
                }
            };

            context.AddRange(experimentos);
            context.SaveChanges();
        }

        [TestMethod()]
        public void CreateTest()
        {
            var novaExperimento = new Experimento
            {
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(5),
                Cepa = "Cepa C",
                IdPesquisador = 3,
            };

            var createdId = experimentoService.Create(novaExperimento);
            Assert.AreEqual(3, experimentoService.GetAll().Count());
            var experimento = experimentoService.Get(createdId);
            Assert.IsNotNull(experimento);
            Assert.AreEqual("Cepa C", experimento.Cepa);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            experimentoService.Delete(1);
            Assert.AreEqual(1, experimentoService.GetAll().Count());
            var experimento = experimentoService.Get(1);
            Assert.IsNull(experimento);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            var experimento = experimentoService.Get(2);
            Assert.IsNotNull(experimento);
            experimento.Cepa = "Cepa Alterada";
            experimentoService.Update(experimento);
            experimento = experimentoService.Get(2);
            Assert.IsNotNull(experimento);
            Assert.AreEqual("Cepa Alterada", experimento.Cepa);
        }

        [TestMethod()]
        public void GetTest()
        {
            var experimento = experimentoService.Get(1);
            Assert.IsNotNull(experimento);
            Assert.AreEqual("Cepa A", experimento.Cepa);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var listaExperimento = experimentoService.GetAll().ToList();
            Assert.IsNotNull(listaExperimento);
            Assert.AreEqual(2, listaExperimento.Count);
            Assert.IsTrue(listaExperimento.Any(e => e.Cepa == "Cepa A"));
            Assert.IsTrue(listaExperimento.Any(e => e.Cepa == "Cepa B"));
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
