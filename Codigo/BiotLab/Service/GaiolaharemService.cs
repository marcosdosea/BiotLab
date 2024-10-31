using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class GaiolaharemService : IGaiolaharemService
    {
        private readonly BiotlabContext context;

        public GaiolaharemService(BiotlabContext context)
        {
            this.context = context;
        }

        public void Create(Gaiolaharem gaiolaharem)
        {
            // Verifica se a entidade já está sendo rastreada pelo contexto
            if (context.Entry(gaiolaharem).State == EntityState.Detached)
            {
                context.Gaiolaharems.Add(gaiolaharem);
            }
            else
            {
                context.Entry(gaiolaharem).State = EntityState.Added;
            }
            context.SaveChanges();
        }

        public void Delete(uint idGaiola, uint idHarem)
        {
            var gaiolaharem = Get(idGaiola, idHarem);
            if (gaiolaharem != null)
            {
                context.Gaiolaharems.Remove(gaiolaharem);
                context.SaveChanges();
            }
        }

        public Gaiolaharem Get(uint idGaiola, uint idHarem)
        {
            return context.Gaiolaharems
                .Include(gh => gh.IdGaiolaNavigation)
                .Include(gh => gh.IdHaremNavigation)
                .Include(gh => gh.IdPesquisadorNavigation)
                .FirstOrDefault(gh => gh.IdGaiola == idGaiola && gh.IdHarem == idHarem);
        }

        public IEnumerable<Gaiolaharem> GetAll()
        {
            return context.Gaiolaharems
                .Include(gh => gh.IdGaiolaNavigation)
                .Include(gh => gh.IdHaremNavigation)
                .Include(gh => gh.IdPesquisadorNavigation)
                .AsNoTracking()
                .ToList();
        }

        public void Update(Gaiolaharem gaiolaharem)
        {
            // Verifica se a entidade está sendo rastreada pelo contexto
            if (context.Entry(gaiolaharem).State == EntityState.Detached)
            {
                context.Gaiolaharems.Attach(gaiolaharem);
                context.Entry(gaiolaharem).State = EntityState.Modified;
            }
            else
            {
                context.Update(gaiolaharem);
            }
            context.SaveChanges();
        }
    }
}
