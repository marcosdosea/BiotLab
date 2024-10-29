using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Core.Service
{
    public interface IFornecedorService
    {
        public uint Create(Fornecedor fornecedor);
        public void Update(Fornecedor fornecedor);
        public void Delete(uint id);
        public IEnumerable<Fornecedor> GetAll();
        public Fornecedor Get(uint id);
    }
}