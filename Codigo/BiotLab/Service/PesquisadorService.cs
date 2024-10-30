using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PesquisadorService : IPesquisadorService
    {
        private readonly BiotlabContext _context;
        public PesquisadorService(BiotlabContext context)
        {
            _context = context;
        }

        public Pesquisador? Buscar(uint id)
        {
            return _context.Pesquisadors.Find(id);
        }

        public uint Create(Pesquisador pesquisador)
        {
            _context.Add(pesquisador);
            _context.SaveChanges(); 
            return pesquisador.Id;
        }

        public void Delete(uint id)
        {
            var pesquisador = _context.Pesquisadors.Find(id);
            if (pesquisador != null)
            {
                _context.Remove(pesquisador);
            }
        }

        public IEnumerable<Pesquisador> GetAll()
        {
            return _context.Pesquisadors.AsNoTracking();
        }

        public void Update(Pesquisador pesquisador)
        {
            _context.Update(pesquisador);
            _context.SaveChanges();
        }
    }
}
