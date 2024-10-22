using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class FornecedorViewModel
    {
        [Display(Name = "Código da Instituição")]
        [Required(ErrorMessage = "O código da instituição é obrigatório")]
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Nome da Instituição")]
        [Required(ErrorMessage = "O nome da instituição é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = null!;

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [RegularExpression(@"\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}", ErrorMessage = "Formato de CNPJ inválido")]
        public string Cnpj { get; set; } = null!;

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "O CEP é obrigatório")]
        [RegularExpression(@"\d{5}-\d{3}", ErrorMessage = "Formato de CEP inválido")]
        public string Cep { get; set; } = null!;

        [Display(Name = "Rua")]
        [StringLength(100, ErrorMessage = "O nome da rua deve ter no máximo 100 caracteres")]
        public string? Rua { get; set; }

        [Display(Name = "Bairro")]
        [StringLength(50, ErrorMessage = "O nome do bairro deve ter no máximo 50 caracteres")]
        public string? Bairro { get; set; }

        [Display(Name = "Cidade")]
        [StringLength(50, ErrorMessage = "O nome da cidade deve ter no máximo 50 caracteres")]
        public string? Cidade { get; set; }

        [Display(Name = "Número")]
        [StringLength(10, ErrorMessage = "O número deve ter no máximo 10 caracteres")]
        public string? Numero { get; set; }

        [Display(Name = "Complemento")]
        [StringLength(100, ErrorMessage = "O complemento deve ter no máximo 100 caracteres")]
        public string? Complemento { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O estado é obrigatório")]
        [StringLength(2, ErrorMessage = "O estado deve ser representado por 2 caracteres")]
        public string Estado { get; set; } = null!;

        [Display(Name = "Telefone 1")]
        [Required(ErrorMessage = "O telefone 1 é obrigatório")]
        [Phone(ErrorMessage = "Formato de telefone inválido")]
        public string Telefone1 { get; set; } = null!;

        [Display(Name = "Telefone 2")]
        [Phone(ErrorMessage = "Formato de telefone inválido")]
        public string? Telefone2 { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = null!;
    }
}
