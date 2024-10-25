using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    internal class ObituarioDTO
    {
        public uint Id { get; set; }

        public DateTime Data { get; set; }

        public uint IdGaiola { get; set; }

        public uint IdPesquisador { get; set; }
    }
}
