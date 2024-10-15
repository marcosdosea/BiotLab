using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IObituarioService
    {
        public void Delete(uint id);
        public uint Create(Obituario obituario);
        public Obituario? Buscar(uint id);
    }
}
