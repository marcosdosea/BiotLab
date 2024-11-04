using Core;
using Core.Service;
using System.Collections.Generic;

namespace Service
{
    public class UsoanestesicoService : IUsoanestesicoService
    {
        private readonly List<Usoanestesico> _usoanestesicos = new();
        private uint _nextId = 1;

        public uint Create(Usoanestesico usoanestesico)
        {
            usoanestesico.Id = _nextId++;
            _usoanestesicos.Add(usoanestesico);
            return usoanestesico.Id;
        }

        public void Update(Usoanestesico usoanestesico)
        {
            var existing = Get(usoanestesico.Id);
            if (existing != null)
            {
                existing.Quantidade = usoanestesico.Quantidade;
                existing.Procedimento = usoanestesico.Procedimento;
                existing.Data = usoanestesico.Data;
                existing.Cepa = usoanestesico.Cepa;
                existing.NumeroAnimais = usoanestesico.NumeroAnimais;
                existing.IdPesquisador = usoanestesico.IdPesquisador;
                existing.IdExperimento = usoanestesico.IdExperimento;
                existing.IdEntrada = usoanestesico.IdEntrada;
                existing.IdAnestesico = usoanestesico.IdAnestesico;
            }
        }

        public void Delete(uint id)
        {
            var usoanestesico = Get(id);
            if (usoanestesico != null)
            {
                _usoanestesicos.Remove(usoanestesico);
            }
        }

        public IEnumerable<Usoanestesico> GetAll()
        {
            return _usoanestesicos;
        }

        public Usoanestesico Get(uint id)
        {
            return _usoanestesicos.Find(u => u.Id == id);
        }
    }
}
