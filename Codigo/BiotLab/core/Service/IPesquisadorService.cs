using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IPesquisadorService
    {
        public void Update(Pesquisador pesquisador);
        public void Delete(uint id);
        public Pesquisador? Buscar(uint id);
        IEnumerable<Pesquisador> GetAll();
        public uint Create(Pesquisador pesquisador);
    }
}
