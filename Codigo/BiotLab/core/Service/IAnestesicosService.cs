using System.Collections.Generic;

namespace Core.Service
{
    public interface IAnestesicosService
    {
        uint Create(Anestesico anestesico);
        void Update(Anestesico anestesico);
        void Delete(uint id);
        bool Validar(uint id);
        Anestesico? Buscar(uint id);
        IEnumerable<Anestesico> GetAll();
    }
}