using System;
using System.Collections.Generic;

namespace Core;

public partial class Obituario
{
    public uint Id { get; set; }

    public DateTime Data { get; set; }

    public uint IdGaiola { get; set; }

    public uint IdPesquisador { get; set; }

    public virtual Gaiola IdGaiolaNavigation { get; set; } = null!;

    public virtual Pesquisador IdPesquisadorNavigation { get; set; } = null!;
}
