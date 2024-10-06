using System;
using System.Collections.Generic;

namespace Core;

public partial class Harem
{
    public uint Id { get; set; }

    public string CodigoInterno { get; set; } = null!;

    public int NumeroMachos { get; set; }

    public int NumeroFemeas { get; set; }

    public DateTime DataNascimento { get; set; }

    /// <summary>
    /// A - ATIVO
    /// I - INATIVO
    /// </summary>
    public string Status { get; set; } = null!;

    public uint IdBioterio { get; set; }

    public virtual ICollection<Gaiolaharem> Gaiolaharems { get; set; } = new List<Gaiolaharem>();

    public virtual Bioterio IdBioterioNavigation { get; set; } = null!;
}
