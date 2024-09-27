using System;
using System.Collections.Generic;

namespace core;

public partial class Entradaanestesico
{
    public int AnestesicoId { get; set; }

    public int EntradaId { get; set; }

    public int Quantidade { get; set; }

    public string Lote { get; set; } = null!;

    public int ValorUnitario { get; set; }

    public int SubTotal { get; set; }

    public virtual Anestesico Anestesico { get; set; } = null!;

    public virtual Entradum Entrada { get; set; } = null!;

    public virtual ICollection<Usoanestesico> Usoanestesicos { get; set; } = new List<Usoanestesico>();
}
