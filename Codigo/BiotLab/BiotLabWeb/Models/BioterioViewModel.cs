using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class BioterioViewModel
    {
        [Display(Name = "Código do Biotério")]
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Nome do Biotério")]
        [Required(ErrorMessage = "O nome do biotério é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
        public string Nome { get; set; } = null!;

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "O CEP é obrigatório")]
        [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "Formato de CEP inválido")]
        public string Cep { get; set; } = null!;

        [Display(Name = "Rua")]
        [StringLength(50, ErrorMessage = "A rua deve ter no máximo 50 caracteres")]
        public string? Rua { get; set; }

        [Display(Name = "Bairro")]
        [StringLength(50, ErrorMessage = "O bairro deve ter no máximo 50 caracteres")]
        public string? Bairro { get; set; }

        [Display(Name = "Cidade")]
        [StringLength(50, ErrorMessage = "A cidade deve ter no máximo 50 caracteres")]
        public string? Cidade { get; set; }

        [Display(Name = "Número")]
        [StringLength(20, ErrorMessage = "O número deve ter no máximo 20 caracteres")]
        public string? Numero { get; set; }

        [Display(Name = "Complemento")]
        [StringLength(50, ErrorMessage = "O complemento deve ter no máximo 50 caracteres")]
        public string? Complemento { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O estado é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O estado deve ter 2 caracteres")]
        public string Estado { get; set; } = null!;

        [Display(Name = "Telefone 1")]
        [Required(ErrorMessage = "O telefone 1 é obrigatório")]
        [StringLength(15, ErrorMessage = "O telefone 1 deve ter no máximo 15 caracteres")]
        public string Telefone1 { get; set; } = null!;

        [Display(Name = "Telefone 2")]
        [StringLength(15, ErrorMessage = "O telefone 2 deve ter no máximo 15 caracteres")]
        public string? Telefone2 { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(50, ErrorMessage = "O email deve ter no máximo 50 caracteres")]
        public string Email { get; set; } = null!;

        [Display(Name = "Instituição")]
        [Required(ErrorMessage = "A instituição é obrigatória")]
        public uint IdInstituicao { get; set; }

        [Display(Name = "Instituição")]
        public string? NomeInstituicao { get; set; }
    }
}