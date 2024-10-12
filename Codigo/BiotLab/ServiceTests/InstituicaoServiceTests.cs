using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class InstituicaoServiceTests
    {
        private BiotlabContext context;
        private IInstituicaoService instituicaoService;

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

            // Criação de uma lista de instituições
            var instituicoes = new List<Instituicao>
            {
                new()
                {
                    Id = 1,
                    Nome = "Instituição 1",
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
                    Nome = "Instituição 2",
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

            context.AddRange(instituicoes);
            context.SaveChanges();

            instituicaoService = new InstituicaoService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Criando uma nova instituição
            var novaInstituicao = new Instituicao
            {
                Id = 3,
                Nome = "Instituição 3",
                Cnpj = "34567890123456",
                Cep = "34567-890",
                Rua = "Rua C",
                Bairro = "Bairro C",
                Cidade = "Cidade C",
                Estado = "Estado C",
                Telefone1 = "3333-3333",
                Email = "email3@exemplo.com"
            };

            var createdId = instituicaoService.Create(novaInstituicao);

            // Verificando a criação
            Assert.AreEqual(3, instituicaoService.GetAll().Count());
            var instituicao = instituicaoService.Get(createdId);
            Assert.AreEqual("Instituição 3", instituicao.Nome);
            Assert.AreEqual("Cidade C", instituicao.Cidade);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Deletando a primeira instituição
            instituicaoService.Delete(1);

            // Verificando se foi removida
            Assert.AreEqual(1, instituicaoService.GetAll().Count());
            var instituicao = instituicaoService.Get(1);
            Assert.IsNull(instituicao);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // Atualizando uma instituição existente
            var instituicao = instituicaoService.Get(2);
            instituicao.Nome = "Instituição Alterada";
            instituicaoService.Update(instituicao);

            // Verificando se a atualização foi bem-sucedida
            instituicao = instituicaoService.Get(2);
            Assert.IsNotNull(instituicao);
            Assert.AreEqual("Instituição Alterada", instituicao.Nome);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Buscando uma instituição pelo ID
            var instituicao = instituicaoService.Get(1);
            Assert.IsNotNull(instituicao);
            Assert.AreEqual("Instituição 1", instituicao.Nome);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Obtendo todas as instituições
            var listaInstituicoes = instituicaoService.GetAll();

            // Verificando se a lista contém todas as instituições esperadas
            Assert.IsInstanceOfType(listaInstituicoes, typeof(IEnumerable<Instituicao>));
            Assert.IsNotNull(listaInstituicoes);
            Assert.AreEqual(2, listaInstituicoes.Count());
            Assert.AreEqual((uint)1, listaInstituicoes.First().Id);
            Assert.AreEqual("Instituição 1", listaInstituicoes.First().Nome);
        }
    }
}
