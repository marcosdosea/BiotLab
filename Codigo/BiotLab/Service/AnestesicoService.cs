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

        private readonly BiotlabContext Context;
        public AnestesicoService(BiotlabContext biotlabContext)
        {
            Context = biotlabContext;
        }

        public Anestesico? Buscar(uint id)
        {
           return Context.Anestesicos.Find(id);
        }

        public uint Create(Anestesico anestesico)
        {
            throw new NotImplementedException();
        }

        public void Delete(uint id)
        {
            var anestesico = Context.Anestesicos.Find(id);
            if (anestesico != null)
            {
                Context.Anestesicos.Remove(anestesico);
                Context.SaveChanges();
            }
        }

        public object GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Anestesico anestesico)
        {
            Context.Update(anestesico);
            Context.SaveChanges();
        }

        public bool Validar(uint id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Anestesico> IAnestesicosService.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
