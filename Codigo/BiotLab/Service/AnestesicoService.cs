using Core;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AnestesicoService : IAnestesicosService
    {

        private readonly BiotlabContext context;
        public AnestesicoService(BiotlabContext biotlabContext)
        {
            context = biotlabContext;
        }

        public Anestesico? Buscar(uint id)
        {
           return context.Anestesicos.Find(id);
        }

        public uint Create(Anestesico anestesico)
        {
            context.Add(anestesico);
            context.SaveChanges();
            return anestesico.Id;
        }

        public void Delete(uint id)
        {
            var anestesico = context.Anestesicos.Find(id);
            if (anestesico != null)
            {
                context.Anestesicos.Remove(anestesico);
                context.SaveChanges();
            }
        }

        public object GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Anestesico anestesico)
        {
            context.Update(anestesico);
            context.SaveChanges();
        }

        public bool Validar(uint id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Anestesico> IAnestesicosService.GetAll()
        {
            return context.Anestesicos.ToList();
        }
    }
}
