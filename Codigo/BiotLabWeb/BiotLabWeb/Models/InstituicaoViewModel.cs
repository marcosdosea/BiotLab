namespace BiotLabWeb.Models
{
    public class InstituicaoViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; } = null!;

        public string Cnpj { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public string? Telefone { get; set; }

        public string Email { get; set; } = null!;
    }
}
