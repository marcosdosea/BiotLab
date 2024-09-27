using System;
using System.Collections.Generic;

namespace core;

public partial class Povoargaiola
{
    public int Id { get; set; }

    public int Codigo { get; set; }

    public string Sexo { get; set; } = null!;

    public int QuantdadeRatos { get; set; }

    public float PesoMedio { get; set; }

    public DateTime DataNascimento { get; set; }

    public DateTime DataChegada { get; set; }

    public string EstadoGaiola { get; set; } = null!;

    public int GaiolaId { get; set; }

    public int HarenId { get; set; }

    public int PesquisadorId { get; set; }

    public virtual Gaiola Gaiola { get; set; } = null!;

    public virtual Haren Haren { get; set; } = null!;

    public virtual Pesquisador Pesquisador { get; set; } = null!;
}
