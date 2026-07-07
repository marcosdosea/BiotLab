using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class GaiolaService : IGaiolaService
    {
        private readonly BiotlabContext context;

        public GaiolaService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Gaiola gaiola)
        {
            context.Gaiolas.Add(gaiola);
            context.SaveChanges();
            return gaiola.Id;
        }

        public void Delete(uint id)
        {
            var gaiola = context.Gaiolas.Find(id);
            if (gaiola != null)
            {
                context.Gaiolas.Remove(gaiola);
                context.SaveChanges();
            }
        }

        public Gaiola? Get(uint id)
        {
            return context.Gaiolas.Find(id);
        }

        public IEnumerable<Gaiola> GetAll()
        {
            return context.Gaiolas
                .AsNoTracking()
                .OrderBy(x => x.CodigoInterno)
                .ToList();
        }

        public void Update(Gaiola gaiola)
        {
            var gaiolaExistente = context.Gaiolas.Find(gaiola.Id);

            if (gaiolaExistente == null)
            {
                throw new InvalidOperationException("Gaiola não encontrada para atualização.");
            }

            context.Entry(gaiolaExistente).CurrentValues.SetValues(gaiola);
            context.SaveChanges();
        }
    }
}