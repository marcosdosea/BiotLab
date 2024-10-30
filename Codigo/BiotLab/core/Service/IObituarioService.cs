using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IObituarioService
    {
        void Delete(uint id);
        uint Create(Obituario obituario);
        Obituario? Buscar(uint id);
        IEnumerable<Obituario> GetAll(); 
    }
}
