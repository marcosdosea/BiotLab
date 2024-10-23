using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class ExperimentoViewModel
    {
        [Display(Name = "Código do Experimento")]
        [Required(ErrorMessage = "O código da experimento é obrigatório")]
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Nome do Pesquisador")]
        [Required(ErrorMessage = "O nome do Pesquisador é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = null!;

        [Display(Name = "DataInicio")]
        [Required(ErrorMessage = "O DataInicio é obrigatório")]
        [RegularExpression(@"\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}", ErrorMessage = "Formato de DataInicio inválido")]
        public string DataInicio { get; set; } = null!;

        [Display(Name = "DataFim")]
        [Required(ErrorMessage = "O DataFim é obrigatório")]
        [RegularExpression(@"\d{5}-\d{3}", ErrorMessage = "Formato de DataFim inválido")]
        public string DataFim { get; set; } = null!;

        [Display(Name = "Cepa")]
        [StringLength(100, ErrorMessage = "O nome da Cepa deve ter no máximo 100 caracteres")]
        public string? Cepa { get; set; }
    }
}
