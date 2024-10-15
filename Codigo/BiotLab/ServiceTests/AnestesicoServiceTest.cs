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
    public class AnestesicoServiceTests
    {
        private BiotlabContext context;
        private IAnestesicosService anestesicoService;

        [TestInitialize]
        public void Initialize()
        {
            // Configurando o banco de dados em memória para testes
            var builder = new DbContextOptionsBuilder<BiotlabContext>();
            builder.UseInMemoryDatabase("Biotlab_Anestesico");
            var options = builder.Options;

            context = new BiotlabContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Criação de Instituicoes e Fornecedores para os Anestesicos
            var instituicoes = new List<Instituicao>
            {
                new Instituicao
                {
                    Id = 1,
                    Nome = "Instituicao A",
                    Cnpj = "12345678000100",
                    Cep = "12345-678",
                    Rua = "Rua A",
                    Bairro = "Bairro A",
                    Cidade = "Cidade A",
                    Numero = "123",
                    Complemento = "Sala 1",
                    Estado = "Estado A",
                    Telefone1 = "1111-1111",
                    Telefone2 = "2222-2222",
                    Email = "instituicaoA@exemplo.com"
                },
                new Instituicao
                {
                    Id = 2,
                    Nome = "Instituicao B",
                    Cnpj = "23456789000111",
                    Cep = "23456-789",
                    Rua = "Rua B",
                    Bairro = "Bairro B",
                    Cidade = "Cidade B",
                    Numero = "456",
                    Complemento = "Sala 2",
                    Estado = "Estado B",
                    Telefone1 = "3333-3333",
                    Telefone2 = "4444-4444",
                    Email = "instituicaoB@exemplo.com"
                }
            };

            var fornecedores = new List<Fornecedor>
            {
                new Fornecedor
                {
                    Id = 1,
                    Nome = "Fornecedor A",
                    Cnpj = "11111111000122",
                    Cep = "12345-111",
                    Rua = "Rua Fornecedor A",
                    Bairro = "Bairro Fornecedor A",
                    Cidade = "Cidade Fornecedor A",
                    Numero = "100",
                    Complemento = "Apto 1",
                    Estado = "Estado A",
                    Telefone1 = "5555-5555",
                    Telefone2 = "6666-6666",
                    Email = "fornecedorA@exemplo.com",
                    IdInstituicao = 1
                },
                new Fornecedor
                {
                    Id = 2,
                    Nome = "Fornecedor B",
                    Cnpj = "22222222000133",
                    Cep = "23456-222",
                    Rua = "Rua Fornecedor B",
                    Bairro = "Bairro Fornecedor B",
                    Cidade = "Cidade Fornecedor B",
                    Numero = "200",
                    Complemento = "Apto 2",
                    Estado = "Estado B",
                    Telefone1 = "7777-7777",
                    Telefone2 = "8888-8888",
                    Email = "fornecedorB@exemplo.com",
                    IdInstituicao = 2
                }
            };

            // Criação de Anestesicos
            var anestesicos = new List<Anestesico>
            {
                new Anestesico
                {
                    Id = 1,
                    Nome = "Anestesico A",
                    Marca = "Marca A",
                    Concentracao = 0.5M,
                    IdInstituicao = 1
                },
                new Anestesico
                {
                    Id = 2,
                    Nome = "Anestesico B",
                    Marca = "Marca B",
                    Concentracao = 1.0M,
                    IdInstituicao = 2
                }
            };

            context.Instituicaos.AddRange(instituicoes);
            context.Fornecedors.AddRange(fornecedores);
            context.Anestesicos.AddRange(anestesicos);
            context.SaveChanges();

            anestesicoService = new AnestesicoService(context);
        }

        [TestMethod()]
        public void BuscarTest()
        {
            // Buscando um anestésico pelo ID
            var anestesico = anestesicoService.Buscar(1);
            Assert.IsNotNull(anestesico);
            Assert.AreEqual("Anestesico A", anestesico.Nome);
            Assert.AreEqual(0.5M, anestesico.Concentracao);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Deletando um anestésico
            anestesicoService.Delete(1);

            // Verificando se foi removido
            var anestesico = anestesicoService.Buscar(1);
            Assert.IsNull(anestesico);
            Assert.AreEqual(1, context.Anestesicos.Count());
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // Atualizando um anestésico
            var anestesico = anestesicoService.Buscar(2);
            anestesico.Nome = "Anestesico B Atualizado";
            anestesico.Concentracao = 2.0M;
            anestesicoService.Update(anestesico);

            // Verificando a atualização
            anestesico = anestesicoService.Buscar(2);
            Assert.IsNotNull(anestesico);
            Assert.AreEqual("Anestesico B Atualizado", anestesico.Nome);
            Assert.AreEqual(2.0M, anestesico.Concentracao);
        }

        [TestMethod()]
        public void Buscar_NonExistentTest()
        {
            // Buscando um anestésico que não existe
            var anestesico = anestesicoService.Buscar(999);
            Assert.IsNull(anestesico);
        }

        [TestMethod()]
        public void ValidarTest_NotImplemented()
        {
            // Verificando se a exceção NotImplementedException é lançada
            Assert.ThrowsException<NotImplementedException>(() => anestesicoService.Validar(1));
        }
    }
}
