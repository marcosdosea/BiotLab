using System;
using System.Collections.Generic;

namespace core;

public partial class Pesquisador
{
    public int Id { get; set; }

    public string Usuario { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Endereco { get; set; } = null!;

    public int Numero { get; set; }

    public string Cidade { get; set; } = null!;

    public string Cep { get; set; } = null!;

    public string? Telefone { get; set; }

    public string Email { get; set; } = null!;

    public int BioterioId { get; set; }

    public virtual Bioterio Bioterio { get; set; } = null!;

    public virtual ICollection<Experimento> Experimentos { get; set; } = new List<Experimento>();

    public virtual ICollection<Obituario> Obituarios { get; set; } = new List<Obituario>();

    public virtual ICollection<Povoargaiola> Povoargaiolas { get; set; } = new List<Povoargaiola>();

    public virtual ICollection<Usoanestesico> Usoanestesicos { get; set; } = new List<Usoanestesico>();
}
