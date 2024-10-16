using Core;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IEntradaanestesicoService
    {
        uint Create(Entradaanestesico entradaAnestesico);
        void Update(Entradaanestesico entradaAnestesico);
        void Delete(uint idEntrada, uint idAnestesico);
        IEnumerable<Entradaanestesico> GetAll();
        Entradaanestesico Get(uint idEntrada, uint idAnestesico);
    }
}
