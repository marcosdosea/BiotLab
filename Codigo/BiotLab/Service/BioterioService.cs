using Core;
using Core.Service;
using Core.DTO;
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
            context.Add(bioterio);
            context.SaveChanges();
            return bioterio.Id;
        }

        public void Delete(uint id)
        {
            var bioterio = context.Bioterios.Find(id);
            if (bioterio != null)
            {
                context.Remove(bioterio);
                context.SaveChanges();
            }
        }

        public void Update(Bioterio bioterio)
        {
            context.Update(bioterio);
            context.SaveChanges();
        }

        public Bioterio? Get(uint id)
        {
            return context.Bioterios.Find(id);
        }

        public IEnumerable<Bioterio> GetAll()
        {
            return context.Bioterios.AsNoTracking();
        }

        public Bioterio get(uint id)
        {
            throw new NotImplementedException();
        }

        /*public IEnumerable<Bioterio> GetByNome(string nome)
        {
            var query = from bioterio in context.Bioterios
                        where bioterio.Nome == StartsWith(nome)
                        orderby bioterio.Nome
                        select bioterio;
            return query.AsNoTracking();
        }
        */

    }
}
