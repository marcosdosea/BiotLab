using Core;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ObituarioService : IObituarioService
    {
        private readonly BiotlabContext context;

        public ObituarioService(BiotlabContext context)
        {
            this.context = context;
        }

        public Obituario? Buscar(uint id)
        {
            return context.Obituarios.Find(id);
        }

        public uint Create(Obituario obituario)
        {
            context.Obituarios.Add(obituario);
            context.SaveChanges();
            return obituario.Id;
        }

        public void Update(Obituario obituario)
        {
            context.Obituarios.Update(obituario);
            context.SaveChanges();
        }

        public void Delete(uint id)
        {
            var obituario = context.Obituarios.Find(id);
            if (obituario != null)
            {
                context.Obituarios.Remove(obituario);
                context.SaveChanges();
            }
        }

        public IEnumerable<Obituario> GetAll()
        {
            return context.Obituarios
                .OrderByDescending(x => x.Data)
                .ToList();
        }

        public IEnumerable<Obituario> GetByPeriodo(DateTime? dataInicio, DateTime? dataFim)
        {
            var query = context.Obituarios.AsQueryable();

            if (dataInicio.HasValue)
            {
                var inicio = dataInicio.Value.Date;
                query = query.Where(x => x.Data >= inicio);
            }

            if (dataFim.HasValue)
            {
                var fimExclusivo = dataFim.Value.Date.AddDays(1);
                query = query.Where(x => x.Data < fimExclusivo);
            }

            return query
                .OrderByDescending(x => x.Data)
                .ToList();
        }
    }
}
