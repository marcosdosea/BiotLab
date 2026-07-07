using Core;

namespace Core.Service
{
    public interface IExperimentoService
    {
        uint Create(Experimento experimento);
        void Update(Experimento experimento);
        void Delete(uint id);
        Experimento? Get(uint id);
        IEnumerable<Experimento> GetAll();
    }
}