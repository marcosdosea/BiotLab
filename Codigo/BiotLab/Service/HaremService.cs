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
            context.Harems.Add(harem);
            context.SaveChanges();
            return harem.Id;
        }

        public void Delete(uint id)
        {
            var harem = context.Harems.Find(id);
            if (harem != null)
            {
                context.Harems.Remove(harem);
                context.SaveChanges();
            }
        }

        public Harem? Get(uint id)
        {
            return context.Harems.Find(id);
        }

        public IEnumerable<Harem> GetAll()
        {
            return context.Harems
                .AsNoTracking()
                .ToList();
        }

        public void Update(Harem harem)
        {
            var haremExistente = context.Harems.Find(harem.Id);

            if (haremExistente == null)
            {
                throw new InvalidOperationException("Harém não encontrado para atualização.");
            }

            context.Entry(haremExistente).CurrentValues.SetValues(harem);
            context.SaveChanges();
        }
    }
}