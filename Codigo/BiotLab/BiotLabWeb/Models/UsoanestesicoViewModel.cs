using System;

namespace BiotLabWeb.Models
{
    public class UsoanestesicoViewModel
    {
        public uint Id { get; set; }

        public decimal Quantidade { get; set; }

        public string Procedimento { get; set; } = null!;

        public DateTime Data { get; set; }

        public string Cepa { get; set; } = null!;

        public int NumeroAnimais { get; set; }

        public uint IdPesquisador { get; set; }

        public uint IdExperimento { get; set; }

        public uint IdEntrada { get; set; }

        public uint IdAnestesico { get; set; }
    }
}
