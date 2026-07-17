using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Areas.Identity.Data
{
    public class ConviteUsuario
    {
        public uint Id { get; set; }

        [Required]
        [StringLength(150)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Perfil { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Codigo { get; set; } = string.Empty;

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        public DateTime? AceitoEm { get; set; }

        [StringLength(450)]
        public string? UsuarioId { get; set; }

        public bool Aceito => AceitoEm.HasValue;
    }
}
