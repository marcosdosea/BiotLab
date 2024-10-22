using System;
using System.Collections.Generic;


namespace Core.DTO
{
    internal class EntradaanestesicoDTO
    {
        public uint IdEntrada { get; set; }
        public uint IdAnestesico { get; set; }
        public decimal Quantidade { get; set; }
        public string Lote { get; set; } = null!;
        public decimal ValorUnitario { get; set; }
        public decimal SubTotal { get; set; }
    }
}