using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Service
{
    public interface IHaremService
    {
        public int Create(Harem harem);
        public void Update(Harem harem);
        public void Delete(int id);
      
        public IEnumerable<Harem> GetAll();
        public Harem Get(int id);
    }
}
    

