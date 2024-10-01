using System;
using System.Collections.Generic;

namespace core;

public partial class Gaiola
{
    public int Id { get; set; }

    public int CodigoGaiola { get; set; }

    public int NumeroRatos { get; set; }

    public string Localizacao { get; set; } = null!;

    public int BioterioId { get; set; }

    public int? ExperimentoId { get; set; }

    /// <summary>
    /// N - NOVA
    /// E - EXPERIMENTO SENDO REALIZADO
    /// F - EXPERIMENTO FINALIZADO
    /// </summary>
    public string Status { get; set; } = null!;

    public virtual Bioterio Bioterio { get; set; } = null!;

    public virtual Experimento? Experimento { get; set; }

    public virtual ICollection<Obituario> Obituarios { get; set; } = new List<Obituario>();

    public virtual ICollection<Povoargaiola> Povoargaiolas { get; set; } = new List<Povoargaiola>();

    public virtual ICollection<Harem> Harens { get; set; } = new List<Harem>();
}
