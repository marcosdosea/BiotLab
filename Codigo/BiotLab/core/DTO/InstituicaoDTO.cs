using System;
using System.Collections.Generic;

namespace Core.DTO
{
    internal class InstituicaoDTO
    {
        public uint Id { get; set; }

        public string Nome { get; set; } = null!;

        public string Cnpj { get; set; } = null!;

        public string Cep { get; set; } = null!;

        public string Estado { get; set; } = null!;
    }
}