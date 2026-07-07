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

            // A instituição é entidade-pai de vários cadastros e o banco usa
            // DeleteBehavior.Restrict. Por isso a exclusão precisa respeitar a ordem
            // das chaves estrangeiras.
            //
            // Ponto importante: Entrada depende de Fornecedor. Então não basta remover
            // somente entradas com IdInstituicao == id; também é necessário remover
            // entradas cujo IdFornecedor pertença a um fornecedor desta instituição.
            // Esse era o motivo do erro fk_Entrada_Fornecedor1.
            using var transaction = context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory"
                ? null
                : context.Database.BeginTransaction();

            try
            {
                var fornecedorIds = context.Fornecedors
                    .Where(f => f.IdInstituicao == id)
                    .Select(f => f.Id)
                    .ToList();

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
                    .Where(e => e.IdInstituicao == id || fornecedorIds.Contains(e.IdFornecedor))
                    .Select(e => e.Id)
                    .ToList();

                var anestesicoIds = context.Anestesicos
                    .Where(a => a.IdInstituicao == id)
                    .Select(a => a.Id)
                    .ToList();

                // 1. Registros mais dependentes.
                var usosAnestesicos = context.Usoanestesicos
                    .Where(ua => entradaIds.Contains(ua.IdEntrada) || anestesicoIds.Contains(ua.IdAnestesico))
                    .ToList();

                context.Usoanestesicos.RemoveRange(usosAnestesicos);
                context.SaveChanges();

                var entradasAnestesicos = context.Entradaanestesicos
                    .Where(ea => entradaIds.Contains(ea.IdEntrada) || anestesicoIds.Contains(ea.IdAnestesico))
                    .ToList();

                context.Entradaanestesicos.RemoveRange(entradasAnestesicos);
                context.SaveChanges();

                var obituarios = context.Obituarios
                    .Where(o => gaiolaIds.Contains(o.IdGaiola))
                    .ToList();

                var gaiolaHarems = context.Gaiolaharems
                    .Where(gh => gaiolaIds.Contains(gh.IdGaiola) || haremIds.Contains(gh.IdHarem))
                    .ToList();

                context.Obituarios.RemoveRange(obituarios);
                context.Gaiolaharems.RemoveRange(gaiolaHarems);
                context.SaveChanges();

                // 2. Cadastros operacionais ligados ao biotério.
                var gaiolas = context.Gaiolas
                    .Where(g => gaiolaIds.Contains(g.Id))
                    .ToList();

                var harems = context.Harems
                    .Where(h => haremIds.Contains(h.Id))
                    .ToList();

                context.Gaiolas.RemoveRange(gaiolas);
                context.Harems.RemoveRange(harems);
                context.SaveChanges();

                var bioterios = context.Bioterios
                    .Where(b => b.IdInstituicao == id)
                    .ToList();

                context.Bioterios.RemoveRange(bioterios);
                context.SaveChanges();

                // 3. Cadastros ligados diretamente à instituição.
                var entradas = context.Entrada
                    .Where(e => entradaIds.Contains(e.Id))
                    .ToList();

                context.Entrada.RemoveRange(entradas);
                context.SaveChanges();

                var anestesicos = context.Anestesicos
                    .Where(a => a.IdInstituicao == id)
                    .ToList();

                context.Anestesicos.RemoveRange(anestesicos);
                context.SaveChanges();

                var fornecedores = context.Fornecedors
                    .Where(f => f.IdInstituicao == id)
                    .ToList();

                context.Fornecedors.RemoveRange(fornecedores);
                context.SaveChanges();

                context.Instituicaos.Remove(instituicao);
                context.SaveChanges();

                transaction?.Commit();
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
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
