using System;
using System.Collections.Generic;

namespace core;

public partial class Obituario
{
    public int Id { get; set; }

    public string DataFalecimento { get; set; } = null!;

    public int GaiolaId { get; set; }

    public int PesquisadorId { get; set; }

    public virtual Gaiola Gaiola { get; set; } = null!;

    public virtual Pesquisador Pesquisador { get; set; } = null!;
}
