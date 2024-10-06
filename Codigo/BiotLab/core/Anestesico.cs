using System;
using System.Collections.Generic;

namespace Core;

public partial class Anestesico
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public decimal Concentracao { get; set; }

    public uint IdInstituicao { get; set; }

    public virtual ICollection<Entradaanestesico> Entradaanestesicos { get; set; } = new List<Entradaanestesico>();

    public virtual Instituicao IdInstituicaoNavigation { get; set; } = null!;
}
