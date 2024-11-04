using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IEntradumService
    {
        IEnumerable<Entradum> GetAll();
        Entradum? Get(uint id);
        void Update(Entradum entradum);
        void Delete(uint id);
        uint Create(Entradum entradum);
    }
}
