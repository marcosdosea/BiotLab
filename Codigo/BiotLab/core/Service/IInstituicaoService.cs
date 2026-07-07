using System.Collections.Generic;

namespace Core.Service
{
    public interface IInstituicaoService
    {
        uint Create(Instituicao instituicao);
        void Update(Instituicao instituicao);
        void Delete(uint id);
        Instituicao? Get(uint id);
        IEnumerable<Instituicao> GetAll();
    }
}