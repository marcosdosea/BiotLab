using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class UsoanestesicoService : IUsoanestesicoService
    {
        private readonly BiotlabContext context;

        public UsoanestesicoService(BiotlabContext context)
        {
            this.context = context;
        }

        public uint Create(Usoanestesico usoanestesico)
        {
            context.Usoanestesicos.Add(usoanestesico);
            context.SaveChanges();
            return usoanestesico.Id;
        }

        public void Update(Usoanestesico usoanestesico)
        {
            var existente = context.Usoanestesicos.Find(usoanestesico.Id);

            if (existente == null)
            {
                throw new InvalidOperationException("Uso de anestésico não encontrado para atualização.");
            }

            existente.Quantidade = usoanestesico.Quantidade;
            existente.Procedimento = usoanestesico.Procedimento;
            existente.Data = usoanestesico.Data;
            existente.Cepa = usoanestesico.Cepa;
            existente.NumeroAnimais = usoanestesico.NumeroAnimais;
            existente.IdPesquisador = usoanestesico.IdPesquisador;
            existente.IdExperimento = usoanestesico.IdExperimento;
            existente.IdEntrada = usoanestesico.IdEntrada;
            existente.IdAnestesico = usoanestesico.IdAnestesico;

            context.SaveChanges();
        }

        public void Delete(uint id)
        {
            var usoanestesico = context.Usoanestesicos.Find(id);
            if (usoanestesico != null)
            {
                context.Usoanestesicos.Remove(usoanestesico);
                context.SaveChanges();
            }
        }

        public IEnumerable<Usoanestesico> GetAll()
        {
            return context.Usoanestesicos
                .Include(u => u.IdPesquisadorNavigation)
                .Include(u => u.IdExperimentoNavigation)
                .Include(u => u.Entradaanestesico)
                    .ThenInclude(ea => ea.IdAnestesicoNavigation)
                .Include(u => u.Entradaanestesico)
                    .ThenInclude(ea => ea.IdEntradaNavigation)
                .AsNoTracking()
                .OrderByDescending(u => u.Data)
                .ThenBy(u => u.Id)
                .ToList();
        }

        public Usoanestesico? Get(uint id)
        {
            return context.Usoanestesicos
                .Include(u => u.IdPesquisadorNavigation)
                .Include(u => u.IdExperimentoNavigation)
                .Include(u => u.Entradaanestesico)
                    .ThenInclude(ea => ea.IdAnestesicoNavigation)
                .Include(u => u.Entradaanestesico)
                    .ThenInclude(ea => ea.IdEntradaNavigation)
                .FirstOrDefault(u => u.Id == id);
        }
    }
}