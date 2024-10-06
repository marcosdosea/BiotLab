using System;
using System.Collections.Generic;

namespace Core;

public partial class Gaiolaharem
{
    public uint IdGaiola { get; set; }

    public uint IdHarem { get; set; }

    public DateTime DataPovoamento { get; set; }

    public uint IdPesquisador { get; set; }

    public virtual Gaiola IdGaiolaNavigation { get; set; } = null!;

    public virtual Harem IdHaremNavigation { get; set; } = null!;

    public virtual Pesquisador IdPesquisadorNavigation { get; set; } = null!;
}
