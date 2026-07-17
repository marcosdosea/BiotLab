namespace BiotLabWeb.Models.AdminUsuarios
{
    public class UsuarioListagemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool EmailConfirmado { get; set; }
        public bool Bloqueado { get; set; }
        public bool EhUsuarioAtual { get; set; }
        public bool EhConvite { get; set; }
    }
}
