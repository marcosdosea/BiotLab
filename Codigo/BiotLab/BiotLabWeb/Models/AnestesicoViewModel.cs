using System;
using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class AnestesicoViewModel
    {
        [Display(Name = "ID do Anestésico")]
        [Required(ErrorMessage = "O ID do anestésico é obrigatório")]
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Nome do Anestésico")]
        [Required(ErrorMessage = "O nome do anestésico é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome do anestésico deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = null!;

        [Display(Name = "Concentração")]
        [Required(ErrorMessage = "A concentração é obrigatória")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Formato de concentração inválido")]
        public string Concentracao { get; set; } = null!;

        [Display(Name = "Fabricante")]
        [Required(ErrorMessage = "O fabricante é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome do fabricante deve ter no máximo 50 caracteres")]
        public string Fabricante { get; set; } = null!;

        [Display(Name = "Data de Validade")]
        [Required(ErrorMessage = "A data de validade é obrigatória")]
        [DataType(DataType.Date, ErrorMessage = "Formato de data inválido")]
        public DateTime DataValidade { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }
    }
}
