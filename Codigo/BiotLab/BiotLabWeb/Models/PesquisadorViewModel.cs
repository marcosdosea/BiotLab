using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class PesquisadorViewModel
    {
        public uint Id { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, ErrorMessage = "O CPF deve ter 11 caracteres.")]
        public string Cpf { get; set; } = null!;

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome pode ter até 100 caracteres.")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(8, ErrorMessage = "O CEP deve ter 8 caracteres.")]
        public string Cep { get; set; } = null!;

        [StringLength(200, ErrorMessage = "A rua pode ter até 200 caracteres.")]
        public string? Rua { get; set; }

        [StringLength(100, ErrorMessage = "O bairro pode ter até 100 caracteres.")]
        public string? Bairro { get; set; }

        [StringLength(100, ErrorMessage = "A cidade pode ter até 100 caracteres.")]
        public string? Cidade { get; set; }

        [StringLength(10, ErrorMessage = "O número pode ter até 10 caracteres.")]
        public string? Numero { get; set; }

        [StringLength(100, ErrorMessage = "O complemento pode ter até 100 caracteres.")]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(2, ErrorMessage = "O estado deve ter 2 caracteres.")]
        public string Estado { get; set; } = null!;

        [Required(ErrorMessage = "O telefone 1 é obrigatório.")]
        [StringLength(15, ErrorMessage = "O telefone 1 pode ter até 15 caracteres.")]
        public string Telefone1 { get; set; } = null!;

        [StringLength(15, ErrorMessage = "O telefone 2 pode ter até 15 caracteres.")]
        public string? Telefone2 { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [StringLength(200, ErrorMessage = "O email pode ter até 200 caracteres.")]
        public string Email { get; set; } = null!;
    }
}
