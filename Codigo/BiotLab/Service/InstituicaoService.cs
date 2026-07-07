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
            context.Instituicaos.Add(instituicao);
            context.SaveChanges();
            return instituicao.Id;
        }

        public void Delete(uint id)
        {
            var instituicao = context.Instituicaos.Find(id);
            if (instituicao == null)
            {
                return;
            }

            // A instituição é uma entidade-pai no modelo. Como o banco está configurado
            // com DeleteBehavior.Restrict, a remoção direta pode falhar quando existem
            // biotérios, fornecedores, entradas ou anestésicos vinculados. Por isso,
            // removemos primeiro os registros dependentes da própria instituição,
            // respeitando a ordem das chaves estrangeiras.
            var bioterioIds = context.Bioterios
                .Where(b => b.IdInstituicao == id)
                .Select(b => b.Id)
                .ToList();

            var gaiolaIds = context.Gaiolas
                .Where(g => bioterioIds.Contains(g.IdBioterio))
                .Select(g => g.Id)
                .ToList();

            var haremIds = context.Harems
                .Where(h => bioterioIds.Contains(h.IdBioterio))
                .Select(h => h.Id)
                .ToList();

            var entradaIds = context.Entrada
                .Where(e => e.IdInstituicao == id)
                .Select(e => e.Id)
                .ToList();

            var anestesicoIds = context.Anestesicos
                .Where(a => a.IdInstituicao == id)
                .Select(a => a.Id)
                .ToList();

            var obituarios = context.Obituarios
                .Where(o => gaiolaIds.Contains(o.IdGaiola))
                .ToList();

            var gaiolaHarems = context.Gaiolaharems
                .Where(gh => gaiolaIds.Contains(gh.IdGaiola) || haremIds.Contains(gh.IdHarem))
                .ToList();

            var usosAnestesicos = context.Usoanestesicos
                .Where(ua => entradaIds.Contains(ua.IdEntrada) || anestesicoIds.Contains(ua.IdAnestesico))
                .ToList();

            var entradasAnestesicos = context.Entradaanestesicos
                .Where(ea => entradaIds.Contains(ea.IdEntrada) || anestesicoIds.Contains(ea.IdAnestesico))
                .ToList();

            var gaiolas = context.Gaiolas
                .Where(g => gaiolaIds.Contains(g.Id))
                .ToList();

            var harems = context.Harems
                .Where(h => haremIds.Contains(h.Id))
                .ToList();

            var bioterios = context.Bioterios
                .Where(b => b.IdInstituicao == id)
                .ToList();

            var entradas = context.Entrada
                .Where(e => e.IdInstituicao == id)
                .ToList();

            var anestesicos = context.Anestesicos
                .Where(a => a.IdInstituicao == id)
                .ToList();

            var fornecedores = context.Fornecedors
                .Where(f => f.IdInstituicao == id)
                .ToList();

            context.Obituarios.RemoveRange(obituarios);
            context.Gaiolaharems.RemoveRange(gaiolaHarems);
            context.Usoanestesicos.RemoveRange(usosAnestesicos);
            context.Entradaanestesicos.RemoveRange(entradasAnestesicos);
            context.Gaiolas.RemoveRange(gaiolas);
            context.Harems.RemoveRange(harems);
            context.Bioterios.RemoveRange(bioterios);
            context.Entrada.RemoveRange(entradas);
            context.Anestesicos.RemoveRange(anestesicos);
            context.Fornecedors.RemoveRange(fornecedores);
            context.Instituicaos.Remove(instituicao);

            context.SaveChanges();
        }

        public Instituicao Get(uint id)
        {
            return context.Instituicaos.Find(id);
        }

        public IEnumerable<Instituicao> GetAll()
        {
            return context.Instituicaos
                .AsNoTracking()
                .OrderBy(x => x.Nome)
                .ToList();
        }

        public void Update(Instituicao instituicao)
        {
            var instituicaoExistente = context.Instituicaos.Find(instituicao.Id);

            if (instituicaoExistente == null)
            {
                throw new InvalidOperationException("Instituição não encontrada para atualização.");
            }

            context.Entry(instituicaoExistente).CurrentValues.SetValues(instituicao);
            context.SaveChanges();
        }
    }
}