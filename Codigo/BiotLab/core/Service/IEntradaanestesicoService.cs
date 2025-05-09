using Core;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IEntradaanestesicoService
    {
       public uint Create(Entradaanestesico entradaAnestesico);
       public void Update(Entradaanestesico entradaAnestesico);
       public void Delete(uint idEntrada, uint idAnestesico);
       public IEnumerable<Entradaanestesico> GetAll();
       public Entradaanestesico Get(uint idEntrada, uint idAnestesico);
    }
}
