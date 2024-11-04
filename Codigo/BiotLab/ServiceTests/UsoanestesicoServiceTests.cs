using Core;
using Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Tests
{
    [TestClass()]
    public class UsoanestesicoServiceTests
    {
        private IUsoanestesicoService usoanestesicoService = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Inicializando o serviço com uma nova lista
            usoanestesicoService = new UsoanestesicoService();
            SeedDatabase(); // Chama o método para popular a lista de testes
        }

        private void SeedDatabase()
        {
            // Criação de uma lista de usos anestésicos
            var usosanestesicos = new List<Usoanestesico>
            {
                new()
                {
                    Id = 1,
                    Quantidade = 10,
                    Procedimento = "Procedimento 1",
                    Data = DateTime.Now,
                    Cepa = "Cepa A",
                    NumeroAnimais = 5,
                    IdPesquisador = 1,
                    IdExperimento = 1,
                    IdEntrada = 1,
                    IdAnestesico = 1
                },
                new()
                {
                    Id = 2,
                    Quantidade = 20,
                    Procedimento = "Procedimento 2",
                    Data = DateTime.Now,
                    Cepa = "Cepa B",
                    NumeroAnimais = 10,
                    IdPesquisador = 2,
                    IdExperimento = 2,
                    IdEntrada = 2,
                    IdAnestesico = 2
                }
            };

            // Adicionando os dados à lista no serviço
            foreach (var usoanestesico in usosanestesicos)
            {
                usoanestesicoService.Create(usoanestesico);
            }
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Criando um novo uso anestésico
            var novoUsoanestesico = new Usoanestesico
            {
                Quantidade = 30,
                Procedimento = "Procedimento 3",
                Data = DateTime.Now,
                Cepa = "Cepa C",
                NumeroAnimais = 15,
                IdPesquisador = 3,
                IdExperimento = 3,
                IdEntrada = 3,
                IdAnestesico = 3
            };

            var createdId = usoanestesicoService.Create(novoUsoanestesico);

            // Verificando a criação
            Assert.AreEqual(3, usoanestesicoService.GetAll().Count());
            var usoanestesico = usoanestesicoService.Get(createdId);
            Assert.IsNotNull(usoanestesico);
            Assert.AreEqual("Procedimento 3", usoanestesico.Procedimento);
            Assert.AreEqual(30, usoanestesico.Quantidade);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Deletando o primeiro uso anestésico
            usoanestesicoService.Delete(1);

            // Verificando se foi removido
            Assert.AreEqual(1, usoanestesicoService.GetAll().Count());

            // O uso anestésico com ID 1 deve ser null após a exclusão
            var usoanestesico = usoanestesicoService.Get(1);
            Assert.IsNull(usoanestesico);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // Atualizando um uso anestésico existente
            var usoAnestesico = usoanestesicoService.Get(2);
            usoAnestesico.Procedimento = "Procedimento Alterado";
            usoAnestesico.Quantidade = 25;
            usoanestesicoService.Update(usoAnestesico);

            // Verificando se a atualização foi bem-sucedida
            usoAnestesico = usoanestesicoService.Get(2);
            Assert.IsNotNull(usoAnestesico);
            Assert.AreEqual("Procedimento Alterado", usoAnestesico.Procedimento);
            Assert.AreEqual(25, usoAnestesico.Quantidade);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Buscando um uso anestésico pelo ID
            var usoanestesico = usoanestesicoService.Get(1);
            Assert.IsNotNull(usoanestesico);
            Assert.AreEqual<string>("Procedimento 1", usoanestesico.Procedimento); 
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Obtendo todos os usos anestésicos
            var listaUsosanestesicos = usoanestesicoService.GetAll();

            // Verificando se a lista contém todos os usos anestésicos esperados
            Assert.IsInstanceOfType(listaUsosanestesicos, typeof(IEnumerable<Usoanestesico>));
            Assert.IsNotNull(listaUsosanestesicos);
            Assert.AreEqual(2, listaUsosanestesicos.Count());
            Assert.AreEqual(1u, listaUsosanestesicos.First().Id);
            Assert.AreEqual<string>("Procedimento 1", listaUsosanestesicos.First().Procedimento);
        }
    }
}
