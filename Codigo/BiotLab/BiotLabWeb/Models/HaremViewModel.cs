using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class HaremViewModel
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

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Origem do Pai")]
        [Required(ErrorMessage = "A origem do pai é obrigatória.")]
        [StringLength(100, ErrorMessage = "A origem do pai deve ter no máximo 100 caracteres.")]
        public string OrigemPai { get; set; } = null!;

        [Display(Name = "Origem da Mãe")]
        [Required(ErrorMessage = "A origem da mãe é obrigatória.")]
        [StringLength(100, ErrorMessage = "A origem da mãe deve ter no máximo 100 caracteres.")]
        public string OrigemMae { get; set; } = null!;

        [Display(Name = "Status")]
        [Required(ErrorMessage = "O status é obrigatório.")]
        [StringLength(1, ErrorMessage = "O status deve ter 1 caractere.")]
        public string Status { get; set; } = null!;

        [Display(Name = "Biotério")]
        [Required(ErrorMessage = "O biotério é obrigatório.")]
        public uint IdBioterio { get; set; }

        [Display(Name = "Biotério")]
        public string? NomeBioterio { get; set; }
    }
}
