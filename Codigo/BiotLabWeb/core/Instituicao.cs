using System;
using System.Collections.Generic;

namespace core;

public partial class Instituicao
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Cnpj { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string? Telefone { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<Anestesico> Anestesicos { get; set; } = new List<Anestesico>();

    public virtual ICollection<Bioterio> Bioterios { get; set; } = new List<Bioterio>();

    public virtual ICollection<Fornecedor> Fornecedors { get; set; } = new List<Fornecedor>();
}
