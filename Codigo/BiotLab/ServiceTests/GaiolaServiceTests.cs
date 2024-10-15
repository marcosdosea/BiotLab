using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Service.Tests
{
    [TestClass()]
    public class GaiolaServiceTests
    {
        private BiotlabContext context;
        private IGaiolaService gaiolaService;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<BiotlabContext>();
            builder.UseInMemoryDatabase("Biotlab_Gaiola");
            var options = builder.Options;

            context = new BiotlabContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var bioterios = new List<Bioterio>
            {
                new Bioterio
                {
                    Id = 1,
                    Nome = "Bioterio A",
                    Cep = "12345-678",
                    Rua = "Rua A",
                    Bairro = "Bairro A",
                    Cidade = "Cidade A",
                    Numero = "123",
                    Complemento = "Sala 1",
                    Estado = "Estado A",
                    Telefone1 = "1111-1111",
                    Telefone2 = "2222-2222",
                    Email = "bioterioA@exemplo.com",
                    IdInstituicao = 1
                },
                new Bioterio
                {
                    Id = 2,
                    Nome = "Bioterio B",
                    Cep = "23456-789",
                    Rua = "Rua B",
                    Bairro = "Bairro B",
                    Cidade = "Cidade B",
                    Numero = "456",
                    Complemento = "Sala 2",
                    Estado = "Estado B",
                    Telefone1 = "3333-3333",
                    Telefone2 = "4444-4444",
                    Email = "bioterioB@exemplo.com",
                    IdInstituicao = 2
                }
            };

            var experimentos = new List<Experimento>
            {
                new Experimento
                {
                    Id = 1,
                    DataInicio = DateTime.Now.AddDays(-10),
                    DataFim = DateTime.Now.AddDays(10),
                    Cepa = "Cepa 1",
                    IdPesquisador = 1
                },
                new Experimento
                {
                    Id = 2,
                    DataInicio = DateTime.Now.AddDays(-20),
                    DataFim = DateTime.Now.AddDays(5),
                    Cepa = "Cepa 2",
                    IdPesquisador = 2
                }
            };
            var pesquisadores = new List<Pesquisador>
            {
                new Pesquisador
                {
                    Id = 1,
                    Cpf = "12345678901",
                    Nome = "Pesquisador A",
                    Cep = "12345-678",
                    Rua = "Rua A",
                    Bairro = "Bairro A",
                    Cidade = "Cidade A",
                    Numero = "123",
                    Complemento = "Apto 1",
                    Estado = "Estado A",
                    Telefone1 = "1111-1111",
                    Telefone2 = "2222-2222",
                    Email = "pesquisadorA@exemplo.com"
                },
                new Pesquisador
                {
                    Id = 2,
                    Cpf = "23456789012",
                    Nome = "Pesquisador B",
                    Cep = "23456-789",
                    Rua = "Rua B",
                    Bairro = "Bairro B",
                    Cidade = "Cidade B",
                    Numero = "456",
                    Complemento = "Apto 2",
                    Estado = "Estado B",
                    Telefone1 = "3333-3333",
                    Telefone2 = "4444-4444",
                    Email = "pesquisadorB@exemplo.com"
                }
            };

            context.Bioterios.AddRange(bioterios);
            context.Experimentos.AddRange(experimentos);
            context.Pesquisadors.AddRange(pesquisadores);
            context.SaveChanges();

            var gaiolas = new List<Gaiola>
            {
                new Gaiola
                {
                    Id = 1,
                    CodigoInterno = "GAI001",
                    NumeroMachos = 10,
                    NumeroFemeas = 12,
                    Etiqueta = "Etiqueta 1",
                    Localizacao = "Setor A",
                    Status = "N",
                    IdBioterio = 1,
                    IdExperimento = 1,
                    IdPesquisador = 1
                },
                new Gaiola
                {
                    Id = 2,
                    CodigoInterno = "GAI002",
                    NumeroMachos = 8,
                    NumeroFemeas = 15,
                    Etiqueta = "Etiqueta 2",
                    Localizacao = "Setor B",
                    Status = "E",
                    IdBioterio = 2,
                    IdExperimento = 2,
                    IdPesquisador = 2
                }
            };

            context.Gaiolas.AddRange(gaiolas);
            context.SaveChanges();

            gaiolaService = new GaiolaService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var novaGaiola = new Gaiola
            {
                Id = 3,
                CodigoInterno = "GAI003",
                NumeroMachos = 5,
                NumeroFemeas = 7,
                Etiqueta = "Etiqueta 3",
                Localizacao = "Setor C",
                Status = "F",
                IdBioterio = 1,
                IdExperimento = null,
                IdPesquisador = 1
            };

            var createdId = gaiolaService.Create(novaGaiola);
            Assert.AreEqual(3, gaiolaService.GetAll().Count());
            var gaiola = gaiolaService.Get(createdId);
            Assert.IsNotNull(gaiola);
            Assert.AreEqual("GAI003", gaiola.CodigoInterno);
            Assert.AreEqual("Setor C", gaiola.Localizacao);
            Assert.AreEqual("F", gaiola.Status);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            gaiolaService.Delete(1);
            Assert.AreEqual(1, gaiolaService.GetAll().Count());
            var gaiola = gaiolaService.Get(1);
            Assert.IsNull(gaiola);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            var gaiola = gaiolaService.Get(2);
            gaiola.Status = "F";
            gaiola.Localizacao = "Setor B - Atualizado";
            gaiolaService.Update(gaiola);
            gaiola = gaiolaService.Get(2);
            Assert.IsNotNull(gaiola);
            Assert.AreEqual("F", gaiola.Status);
            Assert.AreEqual("Setor B - Atualizado", gaiola.Localizacao);
        }

        [TestMethod()]
        public void GetTest()
        {
            var gaiola = gaiolaService.Get(1);
            Assert.IsNotNull(gaiola);
            Assert.AreEqual("GAI001", gaiola.CodigoInterno);
            Assert.AreEqual("Setor A", gaiola.Localizacao);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var listaGaiolas = gaiolaService.GetAll();
            Assert.IsInstanceOfType(listaGaiolas, typeof(IEnumerable<Gaiola>));
            Assert.IsNotNull(listaGaiolas);
            Assert.AreEqual(2, listaGaiolas.Count());
        }

        [TestMethod()]
        public void GetAll_NoGaiolasTest()
        {
            foreach (var gaiola in gaiolaService.GetAll().ToList())
            {
                gaiolaService.Delete(gaiola.Id);
            }

            var listaGaiolas = gaiolaService.GetAll();
            Assert.IsNotNull(listaGaiolas);
            Assert.AreEqual(0, listaGaiolas.Count());
        }

        [TestMethod()]
        public void Get_NonExistentTest()
        {
            var gaiola = gaiolaService.Get(999);
            Assert.IsNull(gaiola);
        }
    }
}
