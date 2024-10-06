using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class InstituicaoService : IInstituicaoService
    {
        private readonly BiotlabContext context;

        public InstituicaoService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Instituicao instituicao)
        {
            context.Add(instituicao);
            context.SaveChanges();
            return instituicao.Id;
        }

        public void Delete(uint id)
        {
            var instituicao = context.Instituicaos.Find(id);
            if (instituicao != null)
            {
                context.Remove(instituicao);
                context.SaveChanges();
            }
        }

        public Instituicao Get(uint id)
        {
            return context.Instituicaos.Find(id);
        }

        public IEnumerable<Instituicao> GetAll()
        {
            return context.Instituicaos.AsNoTracking();
        }

        public void Update(Instituicao instituicao)
        {
            context.Update(instituicao);
            context.SaveChanges();
        }
    }
}
