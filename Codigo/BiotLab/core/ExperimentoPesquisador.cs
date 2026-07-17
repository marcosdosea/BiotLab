using System;
using System.Collections.Generic;

namespace Core;

public partial class ExperimentoPesquisador
{
    public uint IdExperimento { get; set; }

    public uint IdPesquisador { get; set; }

    public virtual Experimento IdExperimentoNavigation { get; set; } = null!;

    public virtual Pesquisador IdPesquisadorNavigation { get; set; } = null!;
}
