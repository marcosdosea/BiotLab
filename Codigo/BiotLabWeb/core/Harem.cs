using System;
using System.Collections.Generic;

namespace core;

public partial class Harem
{
    public int Id { get; set; }

    public int NumeroMacho { get; set; }

    public int NumeroFemea { get; set; }

    /// <summary>
    /// A - ATIVO
    /// I - INATIVO
    /// </summary>
    public string? Status { get; set; }

    public virtual ICollection<Povoargaiola> Povoargaiolas { get; set; } = new List<Povoargaiola>();

    public virtual ICollection<Gaiola> Gaiolas { get; set; } = new List<Gaiola>();
}
