using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class GaiolaService : IGaiolaService
    {
        private readonly BiotlabContext context;

        public GaiolaService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Gaiola gaiola)
        {
            gaiola.CodigoInterno = GerarProximoCodigoInterno();

            context.Gaiolas.Add(gaiola);
            context.SaveChanges();
            return gaiola.Id;
        }

        public void Delete(uint id)
        {
            var gaiola = context.Gaiolas.Find(id);
            if (gaiola != null)
            {
                context.Gaiolas.Remove(gaiola);
                context.SaveChanges();
            }
        }

        public Gaiola? Get(uint id)
        {
            return context.Gaiolas.Find(id);
        }

        public IEnumerable<Gaiola> GetAll()
        {
            return context.Gaiolas
                .AsNoTracking()
                .OrderBy(x => x.CodigoInterno)
                .ToList();
        }

        public void Update(Gaiola gaiola)
        {
            var gaiolaExistente = context.Gaiolas.Find(gaiola.Id);

            if (gaiolaExistente == null)
            {
                throw new InvalidOperationException("Gaiola não encontrada para atualização.");
            }

            gaiolaExistente.NumeroMachos = gaiola.NumeroMachos;
            gaiolaExistente.NumeroFemeas = gaiola.NumeroFemeas;
            gaiolaExistente.Etiqueta = gaiola.Etiqueta;
            gaiolaExistente.Localizacao = gaiola.Localizacao;
            gaiolaExistente.Status = gaiola.Status;
            gaiolaExistente.IdBioterio = gaiola.IdBioterio;
            gaiolaExistente.IdExperimento = gaiola.IdExperimento;
            gaiolaExistente.IdPesquisador = gaiola.IdPesquisador;
            context.SaveChanges();
        }

        public string GerarProximoCodigoInterno()
        {
            var maiorNumero = context.Gaiolas
                .AsNoTracking()
                .Select(g => g.CodigoInterno)
                .ToList()
                .Select(ExtrairNumeroCodigo)
                .DefaultIfEmpty(0)
                .Max();

            string codigo;
            do
            {
                maiorNumero++;
                codigo = $"G{maiorNumero:0000}";
            }
            while (CodigoInternoExiste(codigo));

            return codigo;
        }

        public bool CodigoInternoExiste(string codigoInterno, uint? ignorarId = null)
        {
            return context.Gaiolas.Any(g =>
                g.CodigoInterno == codigoInterno &&
                (!ignorarId.HasValue || g.Id != ignorarId.Value));
        }

        private static int ExtrairNumeroCodigo(string? codigoInterno)
        {
            if (string.IsNullOrWhiteSpace(codigoInterno))
            {
                return 0;
            }

            var texto = codigoInterno.Trim();
            if (texto.StartsWith("G", StringComparison.OrdinalIgnoreCase))
            {
                texto = texto[1..];
            }

            return int.TryParse(texto, out var numero) ? numero : 0;
        }
    }
}
