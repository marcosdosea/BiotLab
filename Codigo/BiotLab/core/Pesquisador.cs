using System;
using System.Collections.Generic;

namespace Core;

public partial class Pesquisador
{
    public uint Id { get; set; }

    public string Cpf { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Cep { get; set; } = null!;

    public string? Rua { get; set; }

    public string? Bairro { get; set; }

    public string? Cidade { get; set; }

    public string? Numero { get; set; }

    public string? Complemento { get; set; }

    public string Estado { get; set; } = null!;

    public string Telefone1 { get; set; } = null!;

    public string? Telefone2 { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<Experimento> Experimentos { get; set; } = new List<Experimento>();

    public virtual ICollection<Gaiolaharem> Gaiolaharems { get; set; } = new List<Gaiolaharem>();

    public virtual ICollection<Gaiola> Gaiolas { get; set; } = new List<Gaiola>();

    public virtual ICollection<Obituario> Obituarios { get; set; } = new List<Obituario>();

    public virtual ICollection<Usoanestesico> Usoanestesicos { get; set; } = new List<Usoanestesico>();
}
