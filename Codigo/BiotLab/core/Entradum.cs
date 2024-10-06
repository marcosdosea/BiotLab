using System;
using System.Collections.Generic;

namespace Core;

public partial class Entradum
{
    public uint Id { get; set; }

    public string NumeroNotaFiscal { get; set; } = null!;

    public DateTime DataEntrada { get; set; }

    public uint IdFornecedor { get; set; }

    public uint IdInstituicao { get; set; }

    public virtual ICollection<Entradaanestesico> Entradaanestesicos { get; set; } = new List<Entradaanestesico>();

    public virtual Fornecedor IdFornecedorNavigation { get; set; } = null!;

    public virtual Instituicao IdInstituicaoNavigation { get; set; } = null!;
}
