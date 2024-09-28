using core;
using core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class GaiolaService : IGaiolaService
    {
        private readonly BiotLabContext context;

        public GaiolaService(BiotLabContext context)
        {
            this.context = context;
        }

        public int Create(Gaiola gaiola)
        {
            context.Add(gaiola);
            context.SaveChanges();
            return gaiola.Id;
        }

        public void Delete(int id)
        {
            var gaiola = context.Gaiolas.Find(id);
            if (gaiola != null)
            {
                context.Remove(gaiola);
                context.SaveChanges();
            }
        }

        public Gaiola Get(int id)
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
