using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Areas.Identity.Data
{
    public class UsuarioIdentity : IdentityUser
    {
        [Required]
        [StringLength(150)]
        public string NomeCompleto { get; set; } = string.Empty;

        [StringLength(30)]
        public string? TipoUsuario { get; set; }
    }
}