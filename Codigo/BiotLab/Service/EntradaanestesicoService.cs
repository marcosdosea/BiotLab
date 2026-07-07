using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class EntradaanestesicoService : IEntradaanestesicoService
    {
        private readonly BiotlabContext context;

        public EntradaanestesicoService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Entradaanestesico entradaAnestesico)
        {
            context.Entradaanestesicos.Add(entradaAnestesico);
            context.SaveChanges();
            return entradaAnestesico.IdEntrada;
        }

        public void Update(Entradaanestesico entradaAnestesico)
        {
            var existente = context.Entradaanestesicos.Find(entradaAnestesico.IdEntrada, entradaAnestesico.IdAnestesico);

            if (existente == null)
            {
                throw new InvalidOperationException("Entrada de anestésico não encontrada para atualização.");
            }

            existente.Quantidade = entradaAnestesico.Quantidade;
            existente.Lote = entradaAnestesico.Lote;
            existente.ValorUnitario = entradaAnestesico.ValorUnitario;
            existente.SubTotal = entradaAnestesico.SubTotal;

            context.SaveChanges();
        }

        public void Delete(uint idEntrada, uint idAnestesico)
        {
            var entradaAnestesico = context.Entradaanestesicos.Find(idEntrada, idAnestesico);
            if (entradaAnestesico != null)
            {
                context.Entradaanestesicos.Remove(entradaAnestesico);
                context.SaveChanges();
            }
        }

        public IEnumerable<Entradaanestesico> GetAll()
        {
            return context.Entradaanestesicos
                .Include(e => e.IdAnestesicoNavigation)
                .Include(e => e.IdEntradaNavigation)
                .AsNoTracking()
                .OrderByDescending(e => e.IdEntradaNavigation.DataEntrada)
                .ThenBy(e => e.IdAnestesicoNavigation.Nome)
                .ToList();
        }

        public Entradaanestesico Get(uint idEntrada, uint idAnestesico)
        {
            return context.Entradaanestesicos
                .Include(e => e.IdAnestesicoNavigation)
                .Include(e => e.IdEntradaNavigation)
                .FirstOrDefault(e => e.IdEntrada == idEntrada && e.IdAnestesico == idAnestesico);
        }
    }
}