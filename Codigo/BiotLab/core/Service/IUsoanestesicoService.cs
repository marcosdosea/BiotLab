using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IUsoanestesicoService
    {
        public uint Create(Usoanestesico usoanestesico);
        public void Update(Usoanestesico usoanestesico);
        public void Delete(uint id);
        public IEnumerable<Usoanestesico> GetAll();
        public Usoanestesico Get(uint id);
    }
}
