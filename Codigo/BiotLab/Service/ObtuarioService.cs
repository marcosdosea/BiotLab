﻿using Core.Service;
using Core;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ObituarioService : IObituarioService
    {
        private readonly BiotlabContext context;

        public ObituarioService(BiotlabContext context)
        {
            this.context = context;
        }

        public Obituario? Buscar(uint id)
        {
            return context.Obituarios.Find(id);
        }

        public uint Create(Obituario obituario)
        {
            context.Add(obituario);
            context.SaveChanges();
            return obituario.Id;
        }

        public void Delete(uint id)
        {
            var obituario = context.Obituarios.Find(id);
            if (obituario != null)
            {
                context.Obituarios.Remove(obituario);
                context.SaveChanges();
            }
        }

        public IEnumerable<Obituario> GetAll() 
        {
            return context.Obituarios.ToList();
        }
    }
}
