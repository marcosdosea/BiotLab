using System;
using System.Collections.Generic;

namespace Core.DTO
{
    internal class ExperimentoDTO
    {
        public uint Id { get; set; }

        public string Titulo { get; set; } = null!;

        public string? Cepa { get; set; }

        public string DataInicio { get; set; } = null!;

        public string DataFim { get; set; } = null!;

        public string IdPesquisador { get; set; } = null!;

        public string Gaiolas { get; set; } = null!;

        public string IdPesquisadorNavigation { get; set; } = null!;

        public string Usoanestesicos { get; set; } = null!;
    }
}
