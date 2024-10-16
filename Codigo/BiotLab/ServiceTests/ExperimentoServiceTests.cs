using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class ExperimentoServiceTests
    {
        private BiotlabContext context;
        private IExperimentoService experimentoService;

        [TestInitialize]
        public void Initialize()
        {
            // Configurando o banco de dados em memória para testes
            var builder = new DbContextOptionsBuilder<BiotlabContext>();
            builder.UseInMemoryDatabase("Biotlab");
            var options = builder.Options;

            context = new BiotlabContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            // Criação de uma lista de experimentos
            var experimentos = new List<Experimento>
            {
                new()
                {
                    Id = 1,
                    Nome = "Experimento 1",
                    Cnpj = "12345678901234",
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
                    Nome = "Experimento 2",
                    Cnpj = "23456789012345",
                    Cep = "23456-789",
                    Rua = "Rua B",
                    Bairro = "Bairro B",
                    Cidade = "Cidade B",
                    Estado = "Estado B",
                    Telefone1 = "2222-2222",
                    Email = "email2@exemplo.com"
                }
            };

            context.AddRange(experimento);
            context.SaveChanges();

            experimentoService = new ExperimentoService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Criando um novo experimento
            var novaExperimento = new Experimento
            {
                Id = 3,
                Nome = "Experimento 3",
                Cnpj = "34567890123456",
                Cep = "34567-890",
                Rua = "Rua C",
                Bairro = "Bairro C",
                Cidade = "Cidade C",
                Estado = "Estado C",
                Telefone1 = "3333-3333",
                Email = "email3@exemplo.com"
            };

            var createdId = experimentoService.Create(novaExperimento);

            // Verificando a criação
            Assert.AreEqual(3, experimentoService.GetAll().Count());
            var experimento = experimentoService.Get(createdId);
            Assert.AreEqual("Experimento 3", experimento.Nome);
            Assert.AreEqual("Cidade C", experimento.Cidade);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Deletando o primeiro Experimento
            experimentoService.Delete(1);

            // Verificando se foi removida
            Assert.AreEqual(1, experimentoService.GetAll().Count());
            var experimento = experimentoService.Get(1);
            Assert.IsNull(experimento);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // Atualizando um Experimento existente
            var experimento = experimentoService.Get(2);
            experimento.Nome = "Experimento Alterado";
            experimentoService.Update(experimento);

            // Verificando se a atualização foi bem-sucedida
            experimento = experimentoService.Get(2);
            Assert.IsNotNull(experimento);
            Assert.AreEqual("Experimento Alterada", experimento.Nome);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Buscando um Experimento pelo ID
            var experimento = experimentoService.Get(1);
            Assert.IsNotNull(experimento);
            Assert.AreEqual("Experimento 1", experimento.Nome);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Obtendo todas os Experimentos
            var listaExperimento = experimentoService.GetAll();

            // Verificando se a lista contém todas os Experimentos esperadas
            Assert.IsInstanceOfType(listaExperimento, typeof(IEnumerable<Experimento>));
            Assert.IsNotNull(listaExperimento);
            Assert.AreEqual(2, listaExperimento.Count());
            Assert.AreEqual((uint)1, listaExperimento.First().Id);
            Assert.AreEqual("Experimento 1", listaExperimento.First().Nome);
        }
    }
}
