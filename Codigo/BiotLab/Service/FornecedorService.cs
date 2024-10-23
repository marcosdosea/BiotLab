using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            context.Add(fornecedor);
            context.SaveChanges();
            return fornecedor.Id;
        }

        public void Delete(uint id)
        {
            var fornecedor = context.Fornecedors.Find(id);
            if (fornecedor != null)
            {
                context.Remove(fornecedor);
                context.SaveChanges();
            }
        }

        public Fornecedor Get(uint id)
        {
            return context.Fornecedors.Find(id);
        }

        public IEnumerable<Fornecedor> GetAll()
        {
            return context.Fornecedors.AsNoTracking();
        }

        public void Update(Fornecedor fornecedor)
        {
            context.Update(fornecedor);
            context.SaveChanges();
        }
    }
}
