using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class GaiolaViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Código Interno")]
        [Required(ErrorMessage = "O código interno é obrigatório.")]
        [StringLength(50, ErrorMessage = "O código interno deve ter no máximo 50 caracteres.")]
        public string CodigoInterno { get; set; } = null!;

        [Display(Name = "Número de Machos")]
        [Range(0, int.MaxValue, ErrorMessage = "Informe um número válido.")]
        public int NumeroMachos { get; set; }

        [Display(Name = "Número de Fêmeas")]
        [Range(0, int.MaxValue, ErrorMessage = "Informe um número válido.")]
        public int NumeroFemeas { get; set; }

        [Display(Name = "Etiqueta")]
        [StringLength(100, ErrorMessage = "A etiqueta deve ter no máximo 100 caracteres.")]
        public string? Etiqueta { get; set; }

        [Display(Name = "Localização")]
        [Required(ErrorMessage = "A localização é obrigatória.")]
        [StringLength(100, ErrorMessage = "A localização deve ter no máximo 100 caracteres.")]
        public string Localizacao { get; set; } = null!;

        [Display(Name = "Status")]
        [Required(ErrorMessage = "O status é obrigatório.")]
        [StringLength(1, ErrorMessage = "O status deve ter 1 caractere.")]
        public string Status { get; set; } = null!;

        [Display(Name = "Biotério")]
        [Required(ErrorMessage = "O biotério é obrigatório.")]
        public uint IdBioterio { get; set; }

        [Display(Name = "Experimento")]
        public uint? IdExperimento { get; set; }

        [Display(Name = "Pesquisador")]
        public uint? IdPesquisador { get; set; }

        [Display(Name = "Biotério")]
        public string? NomeBioterio { get; set; }

        [Display(Name = "Experimento")]
        public string? NomeExperimento { get; set; }

        [Display(Name = "Pesquisador")]
        public string? NomePesquisador { get; set; }
    }
}