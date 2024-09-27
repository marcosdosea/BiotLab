using System;
using System.Collections.Generic;

namespace core;

public partial class Fornecedor
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Cnpj { get; set; } = null!;

    public string Endereco { get; set; } = null!;

    public int Numero { get; set; }

    public string Cidade { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Cep { get; set; } = null!;

    public string? Telefone { get; set; }

    public string Email { get; set; } = null!;

    public string TipoProduto { get; set; } = null!;

    public int CapacidadeFornecimento { get; set; }

    public int InstituicaoId { get; set; }

    public virtual ICollection<Entradum> Entrada { get; set; } = new List<Entradum>();

    public virtual Instituicao Instituicao { get; set; } = null!;
}
