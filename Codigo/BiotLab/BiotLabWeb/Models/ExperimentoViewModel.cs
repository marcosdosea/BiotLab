using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class ExperimentoViewModel
    {
        [Display(Name = "Código do Experimento")]
        [Required(ErrorMessage = "O código do experimento é obrigatório.")]
        public uint Id { get; set; }

        [Display(Name = "Nome do Pesquisador")]
        [Required(ErrorMessage = "O nome do pesquisador é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Data de Início")]
        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [RegularExpression(@"\d{2}/\d{2}/\d{4}", ErrorMessage = "Formato de data de início inválido. Use dd/MM/yyyy.")]
        public string DataInicio { get; set; } = string.Empty;

        [Display(Name = "Data de Fim")]
        [Required(ErrorMessage = "A data de fim é obrigatória.")]
        [RegularExpression(@"\d{2}/\d{2}/\d{4}", ErrorMessage = "Formato de data de fim inválido. Use dd/MM/yyyy.")]
        public string DataFim { get; set; } = string.Empty;

        [Display(Name = "Cepa")]
        [StringLength(50, ErrorMessage = "O nome da cepa deve ter no máximo 50 caracteres.")]
        public string? Cepa { get; set; }

        [Display(Name = "Gaiolas")]
        [StringLength(50, ErrorMessage = "O número das gaiolas deve ter no máximo 50 caracteres.")]
        public string? Gaiolas { get; set; }

        [Display(Name = "ID do Pesquisador")]
        [StringLength(50, ErrorMessage = "O ID do pesquisador deve ter no máximo 50 caracteres.")]
        public string? IdPesquisadorNavigation { get; set; }

        [Display(Name = "Anestésico Usado")]
        [StringLength(10, ErrorMessage = "O nome do anestésico deve ter no máximo 10 caracteres.")]
        public string? Usoanestesicos { get; set; }
    }
}