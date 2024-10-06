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
            context.Add(gaiola);
            context.SaveChanges();
            return gaiola.Id;
        }

        public void Delete(uint id)
        {
            var gaiola = context.Gaiolas.Find(id);
            if (gaiola != null)
            {
                context.Remove(gaiola);
                context.SaveChanges();
            }
        }

        public Gaiola Get(uint id)
        {
            return context.Gaiolas.Find(id);
        }

        public IEnumerable<Gaiola> GetAll()
        {
            return context.Gaiolas.AsNoTracking();
        }

        public void Update(Gaiola gaiola)
        {
            context.Update(gaiola);
            context.SaveChanges();
        }
    }
}
