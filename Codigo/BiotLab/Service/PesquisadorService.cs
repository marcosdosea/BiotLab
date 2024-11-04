using Core;
using Core.Service;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class PesquisadorService : IPesquisadorService
    {
        private readonly BiotlabContext _context;

        public PesquisadorService(BiotlabContext context)
        {
            _context = context;
        }

        public uint Create(Pesquisador pesquisador)
        {
            _context.Pesquisadors.Add(pesquisador);
            _context.SaveChanges();
            return pesquisador.Id; // Retorna o ID do pesquisador criado
        }

        public void Delete(uint id)
        {
            var pesquisador = _context.Pesquisadors.Find(id);
            if (pesquisador != null)
            {
                _context.Pesquisadors.Remove(pesquisador);
                _context.SaveChanges();
            }
        }

        public Pesquisador? Buscar(uint id)
        {
            return _context.Pesquisadors.Find(id);
        }

        public IEnumerable<Pesquisador> GetAll()
        {
            return _context.Pesquisadors.ToList();
        }

        public void Update(Pesquisador pesquisador)
        {
            _context.Pesquisadors.Update(pesquisador);
            _context.SaveChanges();
        }
    }
}
