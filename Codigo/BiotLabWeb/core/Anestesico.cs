using System;
using System.Collections.Generic;

namespace core;

public partial class Anestesico
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public float Concentracao { get; set; }

    public DateTime DataVencimento { get; set; }

    public int InstituicaoId { get; set; }

    public virtual ICollection<Entradaanestesico> Entradaanestesicos { get; set; } = new List<Entradaanestesico>();

    public virtual Instituicao Instituicao { get; set; } = null!;

    public virtual ICollection<Usoanestesico> Usoanestesicos { get; set; } = new List<Usoanestesico>();
}
