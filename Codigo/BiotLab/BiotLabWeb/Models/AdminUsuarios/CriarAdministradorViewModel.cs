using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models.AdminUsuarios
{
    public class CriarAdministradorViewModel
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome completo deve ter no máximo 150 caracteres.")]
        [Display(Name = "Nome completo")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;
    }
}