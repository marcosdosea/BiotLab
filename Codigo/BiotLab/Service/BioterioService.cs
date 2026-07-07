using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class BioterioService : IBioterioService
    {
        private readonly BiotlabContext context;

        public BioterioService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Bioterio bioterio)
        {
            context.Bioterios.Add(bioterio);
            context.SaveChanges();
            return bioterio.Id;
        }

        public void Delete(uint id)
        {
            var bioterio = context.Bioterios.Find(id);
            if (bioterio != null)
            {
                context.Bioterios.Remove(bioterio);
                context.SaveChanges();
            }
        }

        public Bioterio? Get(uint id)
        {
            return context.Bioterios.Find(id);
        }

        public IEnumerable<Bioterio> GetAll()
        {
            return context.Bioterios
                .AsNoTracking()
                .OrderBy(x => x.Nome)
                .ToList();
        }

        public void Update(Bioterio bioterio)
        {
            var bioterioExistente = context.Bioterios.Find(bioterio.Id);

            if (bioterioExistente == null)
            {
                throw new InvalidOperationException("Biotério não encontrado para atualização.");
            }

            context.Entry(bioterioExistente).CurrentValues.SetValues(bioterio);
            context.SaveChanges();
        }
    }
}