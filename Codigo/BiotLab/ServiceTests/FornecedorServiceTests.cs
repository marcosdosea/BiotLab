using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class FornecedorServiceTests
    {
        private BiotlabContext context;
        private IFornecedorService fornecedorService;

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

            // Criação de uma lista de fornecedores
            var fornecedors = new List<Fornecedor>
            {
                new()
                {
                    Id = 1,
                    Nome = "Fornecedor 1",
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
                    Nome = "Fornecedor 2",
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

            context.AddRange(fornecedors);
            context.SaveChanges();

            fornecedorService = new FornecedorService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Criando um novo fornecedor
            var novoFornecedor = new Fornecedor
            {
                Id = 3,
                Nome = "Fornecedor 3",
                Cnpj = "34567890123456",
                Cep = "34567-890",
                Rua = "Rua C",
                Bairro = "Bairro C",
                Cidade = "Cidade C",
                Estado = "Estado C",
                Telefone1 = "3333-3333",
                Email = "email3@exemplo.com"
            };

            var createdId = fornecedorService.Create(novoFornecedor);

            // Verificando a criação
            Assert.AreEqual(3, fornecedorService.GetAll().Count());
            var fornecedor = fornecedorService.Get(createdId);
            Assert.AreEqual("Fornecedor 3", fornecedor.Nome);
            Assert.AreEqual("Cidade C", fornecedor.Cidade);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Deletando o primeiro fornecedor
            fornecedorService.Delete(1);

            // Verificando se foi removido
            Assert.AreEqual(1, fornecedorService.GetAll().Count());
            var fornecedor = fornecedorService.Get(1);
            Assert.IsNull(fornecedor);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // Atualizando um fornecedor existente
            var fornecedor = fornecedorService.Get(2);
            fornecedor.Nome = "Fornecedor Alterado";
            fornecedorService.Update(fornecedor);

            // Verificando se a atualização foi bem-sucedida
            fornecedor = fornecedorService.Get(2);
            Assert.IsNotNull(fornecedor);
            Assert.AreEqual("Fornecedor Alterado", fornecedor.Nome);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Buscando um fornecedor pelo ID
            var fornecedor = fornecedorService.Get(1);
            Assert.IsNotNull(fornecedor);
            Assert.AreEqual("Fornecedor 1", fornecedor.Nome);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Obtendo todos os fornecedores
            var listaFornecedors = fornecedorService.GetAll();

            // Verificando se a lista contém todos os fornecedores esperados
            Assert.IsInstanceOfType(listaFornecedors, typeof(IEnumerable<Fornecedor>));
            Assert.IsNotNull(listaFornecedors);
            Assert.AreEqual(2, listaFornecedors.Count());
            Assert.AreEqual((uint)1, listaFornecedors.First().Id);
            Assert.AreEqual("Fornecedor 1", listaFornecedors.First().Nome);
        }
    }
}
