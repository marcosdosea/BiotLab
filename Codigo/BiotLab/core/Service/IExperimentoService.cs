using Core;

namespace Core.Service
{
    public interface IExperimentoService
    {
        uint Create(Experimento experimento, IEnumerable<uint> idsPesquisadores);
        void Update(Experimento experimento, IEnumerable<uint> idsPesquisadores);
        void Delete(uint id);
        Experimento? Get(uint id);
        IEnumerable<Experimento> GetAll();
    }
}
