using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(entrada == null)
            {
                return;
            }
            context.Entrada.Remove(entrada);
            context.SaveChanges();  
        }

        public Entradum? Get(uint id)
        {
            return context.Entrada.Find(id);
        }

        public IEnumerable<Entradum> GetAll()
        {
            return context.Entrada.AsNoTracking();
        }

        public void Update(Entradum entradum)
        {
            context.Entrada.Update(entradum);
            context.SaveChanges();
        }
    }
}
