using System;
using System.Collections.Generic;

namespace core;

public partial class Entradum
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public int NumeroNotaFiscal { get; set; }

    public DateTime DataEntrada { get; set; }

    public int BioterioId { get; set; }

    public int FornecedorId { get; set; }

    public virtual Bioterio Bioterio { get; set; } = null!;

    public virtual ICollection<Entradaanestesico> Entradaanestesicos { get; set; } = new List<Entradaanestesico>();

    public virtual Fornecedor Fornecedor { get; set; } = null!;
}
