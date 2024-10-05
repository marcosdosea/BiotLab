using core;
using core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class HaremService : IHaremService
    {
        private readonly BiotLabContext context;

        public HaremService(BiotLabContext context)
        {
            this.context = context;
        }

        public int Create(Harem harem)
        {
            context.Add(harem);
            context.SaveChanges();
            return harem.Id;
        }

        public void Delete(int id)
        {
            var harem = context.Harens.Find(id);
            if (harem != null)
            {
                context.Remove(harem);
                context.SaveChanges();
            }
        }

        public Harem Get(int id)
        {
            return context.Harens.Find(id);
        }

        public IEnumerable<Harem> GetAll()
        {
            return context.Harens.AsNoTracking();
        }

        public void Update(Harem harem)
        {
            context.Update(harem);
            context.SaveChanges();
        }
    }
}
