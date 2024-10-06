using System;
using System.Collections.Generic;

namespace Core;

public partial class Fornecedor
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Cnpj { get; set; } = null!;

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

    public uint IdInstituicao { get; set; }

    public virtual ICollection<Entradum> Entrada { get; set; } = new List<Entradum>();

    public virtual Instituicao IdInstituicaoNavigation { get; set; } = null!;
}
