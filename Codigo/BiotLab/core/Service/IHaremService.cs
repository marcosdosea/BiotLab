using Core;

namespace Core.Service
{
    public interface IHaremService
    {
        uint Create(Harem harem);
        void Update(Harem harem);
        void Delete(uint id);
        IEnumerable<Harem> GetAll();
        Harem? Get(uint id);
    }
}