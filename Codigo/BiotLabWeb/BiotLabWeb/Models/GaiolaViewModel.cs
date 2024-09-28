namespace BiotLabWeb.Models
{
    public class GaiolaViewModel
    {
        public int Id { get; set; }

        public int CodigoGaiola { get; set; }

        public int NumeroRatos { get; set; }

        public string Localizacao { get; set; } = null!;

        public int BioterioId { get; set; }

        public int? ExperimentoId { get; set; }

        public string Status { get; set; } = null!;
    }
}
