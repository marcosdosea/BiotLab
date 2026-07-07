using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class EntradumService : IEntradumService
    {
        private readonly BiotlabContext context;

        public EntradumService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Entradum entradum)
        {
            context.Entrada.Add(entradum);
            context.SaveChanges();
            return entradum.Id;
        }

        public void Delete(uint id)
        {
            var entrada = context.Entrada.Find(id);
            if (entrada == null)
            {
                return;
            }

            context.Entrada.Remove(entrada);
            context.SaveChanges();
        }

        public Entradum? Get(uint id)
        {
            return context.Entrada
                .Include(e => e.IdFornecedorNavigation)
                .Include(e => e.IdInstituicaoNavigation)
                .FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Entradum> GetAll()
        {
            return context.Entrada
                .Include(e => e.IdFornecedorNavigation)
                .Include(e => e.IdInstituicaoNavigation)
                .AsNoTracking()
                .OrderByDescending(e => e.DataEntrada)
                .ThenBy(e => e.NumeroNotaFiscal)
                .ToList();
        }

        public void Update(Entradum entradum)
        {
            var existingEntradum = context.Entrada.Find(entradum.Id);

            if (existingEntradum == null)
            {
                throw new InvalidOperationException("Entrada não encontrada para atualização.");
            }

            context.Entry(existingEntradum).CurrentValues.SetValues(entradum);
            context.SaveChanges();
        }
    }
}