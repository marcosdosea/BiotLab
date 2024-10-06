using System;
using System.Collections.Generic;

namespace Core;

public partial class Gaiola
{
    public uint Id { get; set; }

    public string CodigoInterno { get; set; } = null!;

    public int NumeroMachos { get; set; }

    public int NumeroFemeas { get; set; }

    public string? Etiqueta { get; set; }

    public string Localizacao { get; set; } = null!;

    /// <summary>
    /// N - NOVA
    /// E - EXPERIMENTO SENDO REALIZADO
    /// F - EXPERIMENTO FINALIZADO
    /// </summary>
    public string Status { get; set; } = null!;

    public uint IdBioterio { get; set; }

    public uint? IdExperimento { get; set; }

    public uint? IdPesquisador { get; set; }

    public virtual ICollection<Gaiolaharem> Gaiolaharems { get; set; } = new List<Gaiolaharem>();

    public virtual Bioterio IdBioterioNavigation { get; set; } = null!;

    public virtual Experimento? IdExperimentoNavigation { get; set; }

    public virtual Pesquisador? IdPesquisadorNavigation { get; set; }

    public virtual ICollection<Obituario> Obituarios { get; set; } = new List<Obituario>();
}
