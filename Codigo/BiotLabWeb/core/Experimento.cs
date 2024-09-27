using System;
using System.Collections.Generic;

namespace core;

public partial class Experimento
{
    public int Id { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    public int Cepa { get; set; }

    public int PesquisadorId { get; set; }

    public virtual ICollection<Gaiola> Gaiolas { get; set; } = new List<Gaiola>();

    public virtual Pesquisador Pesquisador { get; set; } = null!;

    public virtual ICollection<Usoanestesico> Usoanestesicos { get; set; } = new List<Usoanestesico>();
}
