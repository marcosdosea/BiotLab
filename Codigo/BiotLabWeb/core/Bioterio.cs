using System;
using System.Collections.Generic;

namespace core;

public partial class Bioterio
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public string Endereco { get; set; } = null!;

    public int Numero { get; set; }

    public string Cep { get; set; } = null!;

    public int InstituicaoId { get; set; }

    public virtual ICollection<Entradum> Entrada { get; set; } = new List<Entradum>();

    public virtual ICollection<Gaiola> Gaiolas { get; set; } = new List<Gaiola>();

    public virtual Instituicao Instituicao { get; set; } = null!;

    public virtual ICollection<Pesquisador> Pesquisadors { get; set; } = new List<Pesquisador>();
}
