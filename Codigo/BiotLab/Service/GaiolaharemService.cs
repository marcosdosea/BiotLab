using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

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
            context.Add(gaiolaharem);
            context.SaveChanges();
        }

        public void Delete(uint idGaiola, uint idHarem)
        {
            var gaiolaharem = context.Gaiolaharems.Find(idGaiola, idHarem);
            if (gaiolaharem != null)
            {
                context.Remove(gaiolaharem);
                context.SaveChanges();
            }
        }

        public Gaiolaharem Get(uint idGaiola, uint idHarem)
        {
            return context.Gaiolaharems.Find(idGaiola, idHarem);
        }

        public IEnumerable<Gaiolaharem> GetAll()
        {
            return context.Gaiolaharems.AsNoTracking();
        }

        public void Update(Gaiolaharem gaiolaharem)
        {
            context.Update(gaiolaharem);
            context.SaveChanges();
        }
    }
}
