using System;
using System.Collections.Generic;

namespace Core;

public partial class Experimento
{
    public uint Id { get; set; }

    public string Titulo { get; set; } = null!;

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    public string? Cepa { get; set; }

    public uint? IdPesquisador { get; set; }

    public virtual ICollection<ExperimentoPesquisador> ExperimentoPesquisadores { get; set; } = new List<ExperimentoPesquisador>();

    public virtual ICollection<Gaiola> Gaiolas { get; set; } = new List<Gaiola>();

    public virtual Pesquisador? IdPesquisadorNavigation { get; set; }

    public virtual ICollection<Usoanestesico> Usoanestesicos { get; set; } = new List<Usoanestesico>();
}
