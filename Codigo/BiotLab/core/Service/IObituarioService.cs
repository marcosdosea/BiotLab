using System;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IObituarioService
    {
        uint Create(Obituario obituario);
        void Update(Obituario obituario);
        void Delete(uint id);
        Obituario? Buscar(uint id);
        IEnumerable<Obituario> GetAll();
        IEnumerable<Obituario> GetByPeriodo(DateTime? dataInicio, DateTime? dataFim);
    }
}
