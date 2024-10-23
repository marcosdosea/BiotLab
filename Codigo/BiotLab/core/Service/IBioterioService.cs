using Core;
using System.Dynamic;

namespace Core.Service
{
    public interface IBioterioService
    {
        public uint Create(Bioterio bioterio);
        public void Update(Bioterio bioterio);
        public void Delete(uint id);
        public Bioterio? Get(uint id);
        public IEnumerable<Bioterio> GetAll();
        /*IEnumerable<Bioterio> GetByNome(string nome);*/
        public Bioterio get(uint id);


    }
}
