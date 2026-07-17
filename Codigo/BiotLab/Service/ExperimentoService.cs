using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class ExperimentoService : IExperimentoService
    {
        private readonly BiotlabContext context;

        public ExperimentoService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Experimento experimento, IEnumerable<uint> idsPesquisadores)
        {
            var ids = NormalizarIdsPesquisadores(idsPesquisadores);
            experimento.IdPesquisador = ids.First();
            experimento.ExperimentoPesquisadores = ids
                .Select(idPesquisador => new ExperimentoPesquisador { IdPesquisador = idPesquisador })
                .ToList();

            context.Experimentos.Add(experimento);
            context.SaveChanges();
            return experimento.Id;
        }

        public void Delete(uint id)
        {
            var experimento = context.Experimentos
                .Include(e => e.ExperimentoPesquisadores)
                .FirstOrDefault(e => e.Id == id);

            if (experimento != null)
            {
                if (context.Usoanestesicos.Any(u => u.IdExperimento == id))
                {
                    throw new InvalidOperationException("Este experimento possui registros de uso anestésico vinculados e não pode ser excluído.");
                }

                var gaiolasVinculadas = context.Gaiolas
                    .Where(g => g.IdExperimento == id)
                    .ToList();

                foreach (var gaiola in gaiolasVinculadas)
                {
                    gaiola.IdExperimento = null;
                }

                context.ExperimentoPesquisadores.RemoveRange(experimento.ExperimentoPesquisadores);
                context.Experimentos.Remove(experimento);
                context.SaveChanges();
            }
        }

        public Experimento? Get(uint id)
        {
            return context.Experimentos
                .Include(e => e.ExperimentoPesquisadores)
                .ThenInclude(ep => ep.IdPesquisadorNavigation)
                .FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Experimento> GetAll()
        {
            return context.Experimentos
                .Include(e => e.ExperimentoPesquisadores)
                .ThenInclude(ep => ep.IdPesquisadorNavigation)
                .AsNoTracking()
                .OrderByDescending(x => x.DataInicio)
                .ToList();
        }

        public void Update(Experimento experimento, IEnumerable<uint> idsPesquisadores)
        {
            var ids = NormalizarIdsPesquisadores(idsPesquisadores);
            var experimentoExistente = context.Experimentos
                .Include(e => e.ExperimentoPesquisadores)
                .FirstOrDefault(e => e.Id == experimento.Id);

            if (experimentoExistente == null)
            {
                throw new InvalidOperationException("Experimento não encontrado para atualização.");
            }

            experimentoExistente.Titulo = experimento.Titulo;
            experimentoExistente.Cepa = experimento.Cepa;
            experimentoExistente.DataInicio = experimento.DataInicio;
            experimentoExistente.DataFim = experimento.DataFim;
            experimentoExistente.IdPesquisador = ids.First();

            context.ExperimentoPesquisadores.RemoveRange(experimentoExistente.ExperimentoPesquisadores);
            foreach (var idPesquisador in ids)
            {
                experimentoExistente.ExperimentoPesquisadores.Add(new ExperimentoPesquisador
                {
                    IdExperimento = experimentoExistente.Id,
                    IdPesquisador = idPesquisador
                });
            }

            context.SaveChanges();
        }

        private static List<uint> NormalizarIdsPesquisadores(IEnumerable<uint> idsPesquisadores)
        {
            var ids = idsPesquisadores
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (ids.Count == 0)
            {
                throw new ArgumentException("Informe ao menos um pesquisador.", nameof(idsPesquisadores));
            }

            return ids;
        }
    }
}
