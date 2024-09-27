using System;
using System.Collections.Generic;

namespace core;

public partial class Usoanestesico
{
    public int Id { get; set; }

    public string AnestesicoUsado { get; set; } = null!;

    public decimal Volume { get; set; }

    public string ProcedimetoRealizado { get; set; } = null!;

    public DateTime DataUtilização { get; set; }

    public int NumeroCepa { get; set; }

    public int NumerosAnimalEnvolcido { get; set; }

    public int AnestesicoId { get; set; }

    public int PesquisadorId { get; set; }

    public int ExperimentoId { get; set; }

    public int EntradaAnestesicoAnestesicoId { get; set; }

    public int EntradaAnestesicoEntradaId { get; set; }

    public virtual Anestesico Anestesico { get; set; } = null!;

    public virtual Entradaanestesico Entradaanestesico { get; set; } = null!;

    public virtual Experimento Experimento { get; set; } = null!;

    public virtual Pesquisador Pesquisador { get; set; } = null!;
}
