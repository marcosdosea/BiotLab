using System.ComponentModel.DataAnnotations;
using BiotLabWeb.Areas.Identity.Data;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BiotLabWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class DefinirSenhaConviteModel : PageModel
    {
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly IPesquisadorService _pesquisadorService;
        private readonly IdentityContext _identityContext;

        public DefinirSenhaConviteModel(
            UserManager<UsuarioIdentity> userManager,
            IPesquisadorService pesquisadorService,
            IdentityContext identityContext)
        {
            _userManager = userManager;
            _pesquisadorService = pesquisadorService;
            _identityContext = identityContext;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string Codigo { get; set; } = string.Empty;

            public bool CadastrarPesquisador { get; set; }

            public bool EhEstudante { get; set; }

            public bool PesquisadorJaCadastrado { get; set; }

            [Display(Name = "Nome")]
            public string NomeCompleto { get; set; } = string.Empty;

            [Display(Name = "E-mail")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "A senha e obrigatoria.")]
            [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} caracteres e no maximo {1}.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "A confirmacao de senha e obrigatoria.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "A senha e a confirmacao de senha nao conferem.")]
            [Display(Name = "Confirmar senha")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Display(Name = "CPF")]
            public string? Cpf { get; set; }

            [Display(Name = "CEP")]
            public string? Cep { get; set; }

            [Display(Name = "Rua")]
            [StringLength(50, ErrorMessage = "A rua deve ter no maximo 50 caracteres.")]
            public string? Rua { get; set; }

            [Display(Name = "Numero")]
            [StringLength(20, ErrorMessage = "O numero deve ter no maximo 20 caracteres.")]
            public string? Numero { get; set; }

            [Display(Name = "Bairro")]
            [StringLength(50, ErrorMessage = "O bairro deve ter no maximo 50 caracteres.")]
            public string? Bairro { get; set; }

            [Display(Name = "Complemento")]
            [StringLength(50, ErrorMessage = "O complemento deve ter no maximo 50 caracteres.")]
            public string? Complemento { get; set; }

            [Display(Name = "Cidade")]
            [StringLength(50, ErrorMessage = "A cidade deve ter no maximo 50 caracteres.")]
            public string? Cidade { get; set; }

            [Display(Name = "Estado")]
            [StringLength(2, ErrorMessage = "O estado deve ter no maximo 2 caracteres.")]
            public string? Estado { get; set; }

            [Display(Name = "Telefone 1")]
            [StringLength(15, ErrorMessage = "O telefone 1 deve ter no maximo 15 caracteres.")]
            public string? Telefone1 { get; set; }

            [Display(Name = "Telefone 2")]
            [StringLength(15, ErrorMessage = "O telefone 2 deve ter no maximo 15 caracteres.")]
            public string? Telefone2 { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var convite = await ObterConvitePendenteAsync(codigo);
            if (convite == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            PreencherInputConvite(convite);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var convite = await ObterConvitePendenteAsync(Input.Codigo);
            if (convite == null)
            {
                ModelState.AddModelError(string.Empty, "Convite invalido, ja utilizado ou expirado.");
                return Page();
            }

            PreencherInputConvite(convite);

            var usuarioExistente = await _userManager.FindByEmailAsync(convite.Email);
            if (usuarioExistente != null)
            {
                ModelState.AddModelError(string.Empty, "Ja existe um usuario cadastrado com esse e-mail.");
                return Page();
            }

            if (Input.EhEstudante)
            {
                ValidarDadosPesquisador(convite);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new UsuarioIdentity
            {
                UserName = convite.Email,
                Email = convite.Email,
                NomeCompleto = convite.NomeCompleto,
                TipoUsuario = convite.Perfil,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user, Input.Password);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            var roleResult = await _userManager.AddToRoleAsync(user, convite.Perfil);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            try
            {
                if (Input.EhEstudante)
                {
                    CriarOuAtualizarPesquisador(convite);
                }

                convite.AceitoEm = DateTime.UtcNow;
                convite.UsuarioId = user.Id;
                await _identityContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(user);
                ModelState.AddModelError(string.Empty, "Nao foi possivel concluir o cadastro. Detalhes: " + ex.Message);
                return Page();
            }

            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        private async Task<ConviteUsuario?> ObterConvitePendenteAsync(string? codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return null;
            }

            return await _identityContext.ConvitesUsuarios
                .FirstOrDefaultAsync(c => c.Codigo == codigo && c.AceitoEm == null);
        }

        private void PreencherInputConvite(ConviteUsuario convite)
        {
            var ehEstudante = DeveCadastrarPesquisador(convite);
            var pesquisadorExistente = ObterPesquisadorPorEmail(convite.Email);

            Input.Codigo = convite.Codigo;
            Input.NomeCompleto = convite.NomeCompleto;
            Input.Email = convite.Email;
            Input.EhEstudante = ehEstudante;
            Input.PesquisadorJaCadastrado = pesquisadorExistente != null;
            Input.CadastrarPesquisador = ehEstudante;

            if (pesquisadorExistente != null && string.IsNullOrWhiteSpace(Input.Cpf))
            {
                Input.Cpf = pesquisadorExistente.Cpf;
                Input.Cep = pesquisadorExistente.Cep;
                Input.Rua = pesquisadorExistente.Rua;
                Input.Numero = pesquisadorExistente.Numero;
                Input.Bairro = pesquisadorExistente.Bairro;
                Input.Complemento = pesquisadorExistente.Complemento;
                Input.Cidade = pesquisadorExistente.Cidade;
                Input.Estado = pesquisadorExistente.Estado;
                Input.Telefone1 = pesquisadorExistente.Telefone1;
                Input.Telefone2 = pesquisadorExistente.Telefone2;
            }
        }

        private static bool DeveCadastrarPesquisador(ConviteUsuario convite)
        {
            return string.Equals(convite.Perfil, "Estudante", StringComparison.OrdinalIgnoreCase);
        }

        private void ValidarDadosPesquisador(ConviteUsuario convite)
        {
            if (string.IsNullOrWhiteSpace(convite.NomeCompleto))
            {
                ModelState.AddModelError(string.Empty, "O nome do usuario convidado nao foi informado.");
            }

            if (string.IsNullOrWhiteSpace(convite.Email))
            {
                ModelState.AddModelError(string.Empty, "O e-mail do usuario convidado nao foi informado.");
            }

            var cpf = ApenasDigitos(Input.Cpf);
            var cep = ApenasDigitos(Input.Cep);
            var telefone1 = ApenasDigitos(Input.Telefone1);
            var estado = Input.Estado?.Trim().ToUpperInvariant();
            var pesquisadores = _pesquisadorService.GetAll().ToList();
            var pesquisadorExistente = ObterPesquisadorPorEmail(convite.Email);

            if (string.IsNullOrWhiteSpace(cpf))
            {
                ModelState.AddModelError("Input.Cpf", "O CPF e obrigatorio.");
            }
            else if (cpf.Length != 11)
            {
                ModelState.AddModelError("Input.Cpf", "O CPF deve ter 11 digitos.");
            }
            else if (pesquisadores.Any(p => p.Cpf == cpf && p.Id != pesquisadorExistente?.Id))
            {
                ModelState.AddModelError("Input.Cpf", "Ja existe um pesquisador cadastrado com esse CPF.");
            }

            if (string.IsNullOrWhiteSpace(cep))
            {
                ModelState.AddModelError("Input.Cep", "O CEP e obrigatorio.");
            }
            else if (cep.Length != 8)
            {
                ModelState.AddModelError("Input.Cep", "O CEP deve ter 8 digitos.");
            }

            if (string.IsNullOrWhiteSpace(estado))
            {
                ModelState.AddModelError("Input.Estado", "O estado e obrigatorio.");
            }
            else if (estado.Length != 2)
            {
                ModelState.AddModelError("Input.Estado", "O estado deve ter 2 letras.");
            }

            if (string.IsNullOrWhiteSpace(telefone1))
            {
                ModelState.AddModelError("Input.Telefone1", "O telefone 1 e obrigatorio.");
            }
        }

        private Pesquisador? ObterPesquisadorPorEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            return _pesquisadorService.GetAll()
                .FirstOrDefault(p => string.Equals(p.Email, email, StringComparison.OrdinalIgnoreCase));
        }

        private void CriarOuAtualizarPesquisador(ConviteUsuario convite)
        {
            var pesquisador = ObterPesquisadorPorEmail(convite.Email);

            if (pesquisador == null)
            {
                pesquisador = new Pesquisador();
                PreencherPesquisador(pesquisador, convite);
                _pesquisadorService.Create(pesquisador);
                return;
            }

            PreencherPesquisador(pesquisador, convite);
            _pesquisadorService.Update(pesquisador);
        }

        private void PreencherPesquisador(Pesquisador pesquisador, ConviteUsuario convite)
        {
            pesquisador.Nome = Limitar(convite.NomeCompleto, 50) ?? string.Empty;
            pesquisador.Email = Limitar(convite.Email, 50) ?? string.Empty;
            pesquisador.Cpf = ApenasDigitos(Input.Cpf);
            pesquisador.Cep = ApenasDigitos(Input.Cep);
            pesquisador.Rua = Limitar(Input.Rua, 50);
            pesquisador.Numero = Limitar(Input.Numero, 20);
            pesquisador.Bairro = Limitar(Input.Bairro, 50);
            pesquisador.Complemento = Limitar(Input.Complemento, 50);
            pesquisador.Cidade = Limitar(Input.Cidade, 50);
            pesquisador.Estado = Limitar(Input.Estado?.Trim().ToUpperInvariant(), 2) ?? string.Empty;
            pesquisador.Telefone1 = Limitar(Input.Telefone1, 15) ?? string.Empty;
            pesquisador.Telefone2 = Limitar(Input.Telefone2, 15);
        }

        private static string ApenasDigitos(string? valor)
        {
            return new string((valor ?? string.Empty).Where(char.IsDigit).ToArray());
        }

        private static string? Limitar(string? valor, int tamanhoMaximo)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return null;
            }

            valor = valor.Trim();
            return valor.Length <= tamanhoMaximo ? valor : valor[..tamanhoMaximo];
        }
    }
}
