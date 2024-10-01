using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Service
{
    public interface IInstituicaoService
    {
        public int Create(Instituicao instituicao);
        public void Update(Instituicao instituicao);
        public void Delete(int id);
        public IEnumerable<Instituicao> GetAll();
        public Instituicao Get(int id);
    }
}

