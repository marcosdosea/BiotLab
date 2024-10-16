using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class EntradaanestesicoService : IEntradaanestesicoService
    {
        private readonly BiotlabContext context;

        public EntradaanestesicoService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Entradaanestesico entradaAnestesico)
        {
            context.Entradaanestesicos.Add(entradaAnestesico);
            context.SaveChanges();
            // Retornando o IdEntrada como referência
            return entradaAnestesico.IdEntrada;
        }

        public void Update(Entradaanestesico entradaAnestesico)
        {
            context.Entradaanestesicos.Update(entradaAnestesico);
            context.SaveChanges();
        }

        public void Delete(uint idEntrada, uint idAnestesico)
        {
            var entradaAnestesico = context.Entradaanestesicos.Find(idEntrada, idAnestesico);
            if (entradaAnestesico != null)
            {
                context.Entradaanestesicos.Remove(entradaAnestesico);
                context.SaveChanges();
            }
        }

        public IEnumerable<Entradaanestesico> GetAll()
        {
            return context.Entradaanestesicos
                .Include(e => e.IdAnestesicoNavigation)
                .Include(e => e.IdEntradaNavigation)
                .AsNoTracking()
                .ToList();
        }

        public Entradaanestesico Get(uint idEntrada, uint idAnestesico)
        {
            return context.Entradaanestesicos
                .Include(e => e.IdAnestesicoNavigation)
                .Include(e => e.IdEntradaNavigation)
                .FirstOrDefault(e => e.IdEntrada == idEntrada && e.IdAnestesico == idAnestesico);
        }
    }
}
