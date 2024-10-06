using System;
using System.Collections.Generic;

namespace Core;

public partial class Entradaanestesico
{
    public uint IdEntrada { get; set; }

    public uint IdAnestesico { get; set; }

    public decimal Quantidade { get; set; }

    public string Lote { get; set; } = null!;

    public decimal ValorUnitario { get; set; }

    public decimal SubTotal { get; set; }

    public virtual Anestesico IdAnestesicoNavigation { get; set; } = null!;

    public virtual Entradum IdEntradaNavigation { get; set; } = null!;

    public virtual ICollection<Usoanestesico> Usoanestesicos { get; set; } = new List<Usoanestesico>();
}
