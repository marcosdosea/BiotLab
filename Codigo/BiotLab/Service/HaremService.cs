using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class HaremService : IHaremService
    {
        private readonly BiotlabContext context;

        public HaremService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Harem harem)
        {
            context.Add(harem);
            context.SaveChanges();
            return harem.Id;
        }

        public void Delete(uint id)
        {
            var harem = context.Harems.Find(id);
            if (harem != null)
            {
                context.Remove(harem);
                context.SaveChanges();
            }
        }

        public Harem Get(uint id)
        {
            return context.Harems.Find(id);
        }

        public IEnumerable<Harem> GetAll()
        {
            return context.Harems.AsNoTracking();
        }

        public void Update(Harem harem)
        {
            context.Update(harem);
            context.SaveChanges();
        }
    }
}
