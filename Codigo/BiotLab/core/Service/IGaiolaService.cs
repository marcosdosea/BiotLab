using Core;

namespace Core.Service
{
    public interface IGaiolaService
    {
        public uint Create(Gaiola gaiola);
        public void Update(Gaiola gaiola);
        public void Delete(uint id);
        public IEnumerable<Gaiola> GetAll();
        public Gaiola? Get(uint id);
        public string GerarProximoCodigoInterno();
        public bool CodigoInternoExiste(string codigoInterno, uint? ignorarId = null);
    }
}
