using Core;

namespace Core.Service
{
    public interface IInstituicaoService
    {
        public uint Create(Instituicao instituicao);
        public void Update(Instituicao instituicao);
        public void Delete(uint id);
        public IEnumerable<Instituicao> GetAll();
        public Instituicao Get(uint id);
    }
}

