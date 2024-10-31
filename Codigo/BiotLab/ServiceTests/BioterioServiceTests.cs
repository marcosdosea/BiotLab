using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class BioterioServiceTests
    {
        private BiotlabContext context;
        private IBioterioService bioterioService;

        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            var builder = new DbContextOptionsBuilder<BiotlabContext>();
            builder.UseInMemoryDatabase("Biotlab");
            var options = builder.Options;

            context = new BiotlabContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            var autores = new List<Bioterio>
                {
                new()
                {
                    Id = 1,
                    Nome = "Bioterio 1",
                    Cep = "12345-678",
                    Rua = "Rua A",
                    Bairro = "Bairro A",
                    Cidade = "Cidade A",
                    Estado = "Estado A",
                    Telefone1 = "1111-1111",
                    Email = "email1@exemplo.com"
                },
                new()
                {
                    Id = 2,
                    Nome = "Bioterio 2",
                    Cep = "23456-789",
                    Rua = "Rua B",
                    Bairro = "Bairro B",
                    Cidade = "Cidade B",
                    Estado = "Estado B",
                    Telefone1 = "2222-2222",
                    Email = "email2@exemplo.com"
                }
            };

            context.AddRange(autores);
            context.SaveChanges();

            bioterioService = new BioterioService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Act
            var novoBioterio = new Bioterio
            {
                Id = 3,
                Nome = "Bioterio 3",
                Cep = "23456-789",
                Rua = "Rua C",
                Bairro = "Bairro C",
                Cidade = "Cidade C",
                Estado = "Estado C",
                Telefone1 = "(79)93333-3333",
                Email = "email3@exemplo.com"
            };

            var createdId = bioterioService.Create(novoBioterio);
            // Assert
            Assert.AreEqual(3, bioterioService.GetAll().Count());
            var bioterio = bioterioService.Get(createdId);
            Assert.AreEqual("Instituição 3", bioterio.Nome);
            Assert.AreEqual("Cidade C", bioterio.Cidade);
        }

            [TestMethod()]
            public void DeleteTest()
            {
                // Act
                bioterioService.Delete(2);
                // Assert
                Assert.AreEqual(2, bioterioService.GetAll().Count());
                var autor = bioterioService.Get(2);
                Assert.AreEqual(null, autor);
            }

            [TestMethod()]
            public void UpdateTest()
            {
                //Act 
                var bioterio = bioterioService.Get(3);
                bioterio.Nome = "Bioterio Alterado";
                bioterioService.Update(bioterio);
                //Assert
                bioterio = bioterioService.Get(3);
                Assert.IsNotNull(bioterio);
                Assert.AreEqual("Bioterio Alterado", bioterio.Nome);
            }

            [TestMethod()]
            public void GetTest()
            {
                var bioterio = bioterioService.Get(1);
                Assert.IsNotNull(bioterio);
                Assert.AreEqual("Bioterio 1", bioterio.Nome);
            }

            [TestMethod()]
            public void GetAllTest()
            {
                // Act
                var listaBioterios = bioterioService.GetAll();
                // Assert
                Assert.IsInstanceOfType(listaBioterios, typeof(IEnumerable<Bioterio>));
                Assert.IsNotNull(listaBioterios);
                Assert.AreEqual(3, listaBioterios.Count());
                Assert.AreEqual((uint)1, listaBioterios.First().Id);
                Assert.AreEqual("Machado de Assis", listaBioterios.First().Nome);
            }

        }
    }