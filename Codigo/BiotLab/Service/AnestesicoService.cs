using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

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
            context.Anestesicos.Add(anestesico);
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

        public IEnumerable<Anestesico> GetAll()
        {
            return context.Anestesicos
                .AsNoTracking()
                .OrderBy(a => a.Nome)
                .ToList();
        }

        public void Update(Anestesico anestesico)
        {
            var anestesicoExistente = context.Anestesicos.Find(anestesico.Id);

            if (anestesicoExistente == null)
            {
                throw new InvalidOperationException("Anestésico não encontrado para atualização.");
            }

            context.Entry(anestesicoExistente).CurrentValues.SetValues(anestesico);
            context.SaveChanges();
        }

        public bool Validar(uint id)
        {
            return context.Anestesicos.Any(a => a.Id == id);
        }
    }
}