using Core;

namespace Core.Service
{
    public interface IExperimentoService
    {
        public uint Create(Experimento experimento);
        public void Update(Experimento experimento);
        public void Delete(uint id);
        public IEnumerable<Experimento> GetAll();
        public Experimento Get(uint id);
    }
}
