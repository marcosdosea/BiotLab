using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Implementa os serviços para manter os dados de Haren
    /// </summary>
    public class HarenService : IHarenService 
    {
        private readonly BiotLabWebContext context;

        public HarenService(BiotLabWebContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo Haren na base de dados 
        /// </summary>
        /// <param name="haren">dados do Haren</param>
        /// <returns>id gerado</returns>
        public int Create(Haren haren)
        {
            context.Add(haren);
            context.SaveChanges();
            return haren.Id;
        }

        /// <summary>
        /// Remover Haren da base de dados
        /// </summary>
        /// <param name="id">id a ser removido</param>
        public void Delete(int id)
        {
            var haren = context.Haren.Find(id);
            if (haren != null)
            {
                context.Remove(haren);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Atualizar dados do Haren
        /// </summary>
        /// <param name="haren">novos dados do Haren</param>
        public void Edit(Haren haren)
        {
            context.Update(haren);
            context.SaveChanges();
        }

        /// <summary>
        /// Obter os dados de um Haren na base de dados
        /// </summary>
        /// <param name="id">id do Haren</param>
        /// <returns>Dados do Haren</returns>
        public Haren? Get(int id)
        {
            return context.Haren.Find(id);
        }

        /// <summary>
        /// Obter dados de todos os Harens
        /// </summary>
        /// <returns>lista de Harens</returns>
        public IEnumerable<Haren> GetAll()
        {
            return context.Haren.AsNoTracking().ToList();
        }

        /// <summary>
        /// Obter Harens que possuem um status específico
        /// </summary>
        /// <param name="status">status do Haren</param>
        /// <returns>lista de Harens</returns>
        public IEnumerable<Haren> GetByStatus(string status)
        {
            return context.Haren
                .Where(h => h.Status == status)
                .AsNoTracking()
                .ToList();
        }
    }
}
