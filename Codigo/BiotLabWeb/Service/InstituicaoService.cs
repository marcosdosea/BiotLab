using core;
using core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class InstituicaoService : IInstituicaoService
    {
        private readonly BiotLabContext context;

        public InstituicaoService(BiotLabContext context)
        {
            this.context = context;
        }

        public int Create(Instituicao instituicao)
        {
            context.Add(instituicao);
            context.SaveChanges();
            return instituicao.Id;
        }

        public void Delete(int id)
        {
            var instituicao = context.Instituicaos.Find(id);
            if (instituicao != null)
            {
                context.Remove(instituicao);
                context.SaveChanges();
            }
        }

        public Instituicao Get(int id)
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
