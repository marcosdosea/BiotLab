using System;
using System.Collections.Generic;

namespace Core.DTO
{
    internal class InstituicaoDTO
    {
        public uint Id { get; set; }

        public required string Nome { get; set; }

        public required string Cnpj { get; set; }

        public required string Cep { get; set; }

        public required string Estado { get; set; }
    }
}
