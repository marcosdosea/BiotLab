using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class AnestesicoViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome pode ter no máximo 50 caracteres.")]
        public string Nome { get; set; } = null!;

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "A marca é obrigatória.")]
        [StringLength(50, ErrorMessage = "A marca pode ter no máximo 50 caracteres.")]
        public string Marca { get; set; } = null!;

        [Display(Name = "Concentração")]
        [Required(ErrorMessage = "A concentração é obrigatória.")]
        public decimal Concentracao { get; set; }

        [Display(Name = "Instituição")]
        [Required(ErrorMessage = "A instituição é obrigatória.")]
        public uint IdInstituicao { get; set; }

        [Display(Name = "Instituição")]
        public string? NomeInstituicao { get; set; }
    }
}