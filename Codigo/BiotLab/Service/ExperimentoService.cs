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
            context.Experimentos.Add(experimento);
            context.SaveChanges();
            return experimento.Id;
        }

        public void Delete(uint id)
        {
            var experimento = context.Experimentos.Find(id);
            if (experimento != null)
            {
                context.Experimentos.Remove(experimento);
                context.SaveChanges();
            }
        }

        public Experimento? Get(uint id)
        {
            return context.Experimentos.Find(id);
        }

        public IEnumerable<Experimento> GetAll()
        {
            return context.Experimentos
                .AsNoTracking()
                .OrderByDescending(x => x.DataInicio)
                .ToList();
        }

        public void Update(Experimento experimento)
        {
            var experimentoExistente = context.Experimentos.Find(experimento.Id);

            if (experimentoExistente == null)
            {
                throw new InvalidOperationException("Experimento não encontrado para atualização.");
            }

            context.Entry(experimentoExistente).CurrentValues.SetValues(experimento);
            context.SaveChanges();
        }
    }
}