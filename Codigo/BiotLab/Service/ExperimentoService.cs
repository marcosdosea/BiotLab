using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class ExperimentoService : IExperimentoService
    {
        private readonly BiotlabContext context;

        public ExperimentoService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Experimento experimento)
        {
            context.Add(experimento);
            context.SaveChanges();
            return experimento.Id;
        }

        public void Delete(uint id)
        {
            var experimento = context.Experimentos.Find(id);
            if (experimento != null)
            {
                context.Remove(experimento);
                context.SaveChanges();
            }
        }

        public Experimento Get(uint id)
        {
            return context.Experimentos.Find(id);
        }

        public IEnumerable<Experimento> GetAll()
        {
            return context.Experimentos.AsNoTracking();
        }

        public void Update(Experimento experimento)
        {
            context.Update(experimento);
            context.SaveChanges();
        }
    }
}