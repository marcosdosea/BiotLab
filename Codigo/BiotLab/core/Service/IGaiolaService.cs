using Core;

namespace Core.Service
{
    public interface IGaiolaService
    {
        public uint Create(Gaiola gaiola);
        public void Update(Gaiola gaiola);
        public void Delete(uint id);
        public IEnumerable<Gaiola> GetAll();
        public Gaiola Get(uint id);
    }
}
