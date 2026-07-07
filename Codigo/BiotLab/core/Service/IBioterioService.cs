using Core;

namespace Core.Service
{
    public interface IBioterioService
    {
        uint Create(Bioterio bioterio);
        void Update(Bioterio bioterio);
        void Delete(uint id);
        Bioterio? Get(uint id);
        IEnumerable<Bioterio> GetAll();
    }
}