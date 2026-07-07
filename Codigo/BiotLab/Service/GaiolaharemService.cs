using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class GaiolaharemService : IGaiolaharemService
    {
        private readonly BiotlabContext context;

        public GaiolaharemService(BiotlabContext context)
        {
            this.context = context;
        }

        public void Create(Gaiolaharem gaiolaharem)
        {
            var existente = context.Gaiolaharems.Find(gaiolaharem.IdGaiola, gaiolaharem.IdHarem);
            if (existente != null)
            {
                throw new InvalidOperationException("Já existe um vínculo entre esta gaiola e este berçário.");
            }

            context.Gaiolaharems.Add(gaiolaharem);
            context.SaveChanges();
        }

        public void Delete(uint idGaiola, uint idHarem)
        {
            var gaiolaharem = context.Gaiolaharems.Find(idGaiola, idHarem);
            if (gaiolaharem != null)
            {
                context.Gaiolaharems.Remove(gaiolaharem);
                context.SaveChanges();
            }
        }

        public Gaiolaharem Get(uint idGaiola, uint idHarem)
        {
            return context.Gaiolaharems
                .Include(gh => gh.IdGaiolaNavigation)
                .Include(gh => gh.IdHaremNavigation)
                .Include(gh => gh.IdPesquisadorNavigation)
                .FirstOrDefault(gh => gh.IdGaiola == idGaiola && gh.IdHarem == idHarem);
        }

        public IEnumerable<Gaiolaharem> GetAll()
        {
            return context.Gaiolaharems
                .Include(gh => gh.IdGaiolaNavigation)
                .Include(gh => gh.IdHaremNavigation)
                .Include(gh => gh.IdPesquisadorNavigation)
                .AsNoTracking()
                .OrderBy(gh => gh.IdGaiolaNavigation.CodigoInterno)
                .ThenBy(gh => gh.IdHaremNavigation.CodigoInterno)
                .ToList();
        }

        public void Update(Gaiolaharem gaiolaharem)
        {
            var existente = context.Gaiolaharems.Find(gaiolaharem.IdGaiola, gaiolaharem.IdHarem);

            if (existente == null)
            {
                throw new InvalidOperationException("Vínculo gaiola-berçário não encontrado para atualização.");
            }

            existente.DataPovoamento = gaiolaharem.DataPovoamento;
            existente.IdPesquisador = gaiolaharem.IdPesquisador;

            context.SaveChanges();
        }
    }
}
