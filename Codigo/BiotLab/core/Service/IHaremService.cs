using Core;

namespace Core.Service
{
    public interface IHaremService
    {
        public uint Create(Harem harem);
        public void Update(Harem harem);
        public void Delete(uint id);

        public IEnumerable<Harem> GetAll();
        public Harem Get(uint id);
    }
}


