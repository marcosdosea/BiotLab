using System;
using System.Collections.Generic;

namespace Core;

public partial class Usoanestesico
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

    public virtual Entradaanestesico Entradaanestesico { get; set; } = null!;

    public virtual Experimento IdExperimentoNavigation { get; set; } = null!;

    public virtual Pesquisador IdPesquisadorNavigation { get; set; } = null!;
}
