using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Service
{
    public interface IGaiolaService
    {
        public int Create(Gaiola gaiola);
        public void Update(Gaiola gaiola);
        public void Delete(int id);
        public IEnumerable<Gaiola> GetAll();
        public Gaiola Get(int id);
    }
}
