using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class FornecedorService : IFornecedorService
    {
        private readonly BiotlabContext context;

        public FornecedorService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Fornecedor fornecedor)
        {
            context.Fornecedors.Add(fornecedor);
            context.SaveChanges();
            return fornecedor.Id;
        }

        public void Delete(uint id)
        {
            var fornecedor = context.Fornecedors.Find(id);
            if (fornecedor != null)
            {
                context.Fornecedors.Remove(fornecedor);
                context.SaveChanges();
            }
        }

        public Fornecedor? Get(uint id)
        {
            return context.Fornecedors
                .Include(f => f.IdInstituicaoNavigation)
                .FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<Fornecedor> GetAll()
        {
            return context.Fornecedors
                .Include(f => f.IdInstituicaoNavigation)
                .AsNoTracking()
                .OrderBy(f => f.Nome)
                .ToList();
        }

        public void Update(Fornecedor fornecedor)
        {
            var fornecedorExistente = context.Fornecedors.Find(fornecedor.Id);

            if (fornecedorExistente == null)
            {
                throw new InvalidOperationException("Fornecedor não encontrado para atualização.");
            }

            context.Entry(fornecedorExistente).CurrentValues.SetValues(fornecedor);
            context.SaveChanges();
        }
    }
}