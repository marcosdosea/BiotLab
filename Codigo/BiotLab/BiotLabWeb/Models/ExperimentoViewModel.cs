using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class ExperimentoViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; } = null!;

        [Display(Name = "Cepa")]
        [StringLength(100, ErrorMessage = "A cepa deve ter no máximo 100 caracteres.")]
        public string? Cepa { get; set; }

        [Display(Name = "Data de Início")]
        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data de Fim")]
        [Required(ErrorMessage = "A data de fim é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        [Display(Name = "Pesquisadores")]
        public List<uint> IdsPesquisadores { get; set; } = new();

        [Display(Name = "Pesquisadores")]
        public List<string>? NomesPesquisadores { get; set; }
    }
}
