using BiotLabWeb.Areas.Identity.Data;
using BiotLabWeb.Models.AdminUsuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;

namespace BiotLabWeb.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminUsuariosController : Controller
    {
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IdentityContext _identityContext;

        public AdminUsuariosController(
            UserManager<UsuarioIdentity> userManager,
            IEmailSender emailSender,
            IConfiguration configuration,
            IdentityContext identityContext)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _identityContext = identityContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? termo)
        {
            var usuarioAtual = await _userManager.GetUserAsync(User);
            var usuarios = await _userManager.Users.ToListAsync();
            var lista = new List<UsuarioListagemViewModel>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);

                lista.Add(new UsuarioListagemViewModel
                {
                    Id = usuario.Id,
                    NomeCompleto = string.IsNullOrWhiteSpace(usuario.NomeCompleto) ? "(Sem nome)" : usuario.NomeCompleto,
                    Email = usuario.Email ?? "",
                    EmailConfirmado = usuario.EmailConfirmed,
                    Status = usuario.EmailConfirmed ? "Aceito" : "Pendente",
                    Perfil = roles.Contains("Administrador") ? "Administrador"
                           : roles.Contains("Estudante") ? "Estudante"
                           : "Sem perfil",
                    Bloqueado = usuario.LockoutEnd.HasValue && usuario.LockoutEnd.Value > DateTimeOffset.UtcNow,
                    EhUsuarioAtual = usuarioAtual != null && usuario.Id == usuarioAtual.Id,
                    EhConvite = false
                });
            }

            var convitesPendentes = await _identityContext.ConvitesUsuarios
                .AsNoTracking()
                .Where(c => c.AceitoEm == null)
                .ToListAsync();

            foreach (var convite in convitesPendentes)
            {
                lista.Add(new UsuarioListagemViewModel
                {
                    Id = MontarIdConvite(convite.Id),
                    NomeCompleto = convite.NomeCompleto,
                    Email = convite.Email,
                    Perfil = convite.Perfil,
                    Status = "Pendente",
                    EmailConfirmado = false,
                    Bloqueado = false,
                    EhUsuarioAtual = false,
                    EhConvite = true
                });
            }

            if (!string.IsNullOrWhiteSpace(termo))
            {
                termo = termo.Trim().ToLower();

                lista = lista
                    .Where(x =>
                        (!string.IsNullOrWhiteSpace(x.NomeCompleto) && x.NomeCompleto.ToLower().Contains(termo)) ||
                        (!string.IsNullOrWhiteSpace(x.Email) && x.Email.ToLower().Contains(termo)))
                    .ToList();
            }

            var ordenado = lista
                .OrderByDescending(x => x.Perfil == "Administrador")
                .ThenBy(x => x.Status == "Pendente" ? 0 : 1)
                .ThenBy(x => x.NomeCompleto)
                .ToList();

            ViewBag.Termo = termo;

            return View(ordenado);
        }

        [HttpGet]
        public IActionResult NovoAdministrador()
        {
            return View(new CriarUsuarioConviteViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovoAdministrador(CriarUsuarioConviteViewModel model)
        {
            return await EnviarConviteUsuarioAsync(model, "Administrador", nameof(NovoAdministrador));
        }

        [HttpGet]
        public IActionResult NovoEstudante()
        {
            return View(new CriarUsuarioConviteViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovoEstudante(CriarUsuarioConviteViewModel model)
        {
            return await EnviarConviteUsuarioAsync(model, "Estudante", nameof(NovoEstudante));
        }

        private async Task<IActionResult> EnviarConviteUsuarioAsync(
            CriarUsuarioConviteViewModel model,
            string perfil,
            string viewName)
        {
            if (!ModelState.IsValid)
            {
                return View(viewName, model);
            }

            var email = model.Email.Trim();
            var nome = model.NomeCompleto.Trim();

            var usuarioExistente = await _userManager.FindByEmailAsync(email);
            if (usuarioExistente != null)
            {
                ModelState.AddModelError(string.Empty, "Ja existe um usuario cadastrado com esse e-mail.");
                return View(viewName, model);
            }

            var convite = await _identityContext.ConvitesUsuarios
                .FirstOrDefaultAsync(c => c.Email == email && c.AceitoEm == null);

            if (convite == null)
            {
                convite = new ConviteUsuario
                {
                    NomeCompleto = nome,
                    Email = email,
                    Perfil = perfil,
                    Codigo = GerarCodigoConvite(),
                    CriadoEm = DateTime.UtcNow
                };

                _identityContext.ConvitesUsuarios.Add(convite);
            }
            else
            {
                convite.NomeCompleto = nome;
                convite.Perfil = perfil;
                convite.Codigo = GerarCodigoConvite();
                convite.CriadoEm = DateTime.UtcNow;
            }

            await _identityContext.SaveChangesAsync();

            var callbackPath = Url.Page(
                "/Account/DefinirSenhaConvite",
                pageHandler: null,
                values: new
                {
                    area = "Identity",
                    codigo = convite.Codigo
                },
                protocol: null);

            var appBaseUrl = _configuration["App:BaseUrl"]?.Trim().TrimEnd('/');
            var callbackUrl = !string.IsNullOrWhiteSpace(appBaseUrl)
                ? $"{appBaseUrl}{callbackPath}"
                : Url.Page(
                    "/Account/DefinirSenhaConvite",
                    pageHandler: null,
                    values: new
                    {
                        area = "Identity",
                        codigo = convite.Codigo
                    },
                    protocol: Request.Scheme);

            var perfilMinusculo = perfil.ToLowerInvariant();

            try
            {
                await _emailSender.SendEmailAsync(
                    email,
                    $"Convite de {perfilMinusculo} - BiotLab",
                    $@"
                    <div style='font-family: Arial, sans-serif; line-height:1.6;'>
                        <h2>Convite para {perfilMinusculo} do BiotLab</h2>
                        <p>Ola, {HtmlEncoder.Default.Encode(nome)}.</p>
                        <p>Voce foi convidado para acessar o BiotLab como <strong>{HtmlEncoder.Default.Encode(perfil)}</strong>.</p>
                        <p>Clique no botao abaixo para preencher seus dados e definir sua senha:</p>
                        <p>
                            <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'
                               style='display:inline-block;padding:10px 16px;background:#2563eb;color:#fff;text-decoration:none;border-radius:8px;'>
                               Aceitar convite
                            </a>
                        </p>
                        <p>Se voce nao esperava este convite, ignore este e-mail.</p>
                    </div>");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"O convite nao pode ser enviado: {ex.Message}");
                return View(viewName, model);
            }

            return RedirectToAction(nameof(ConviteEnviado), new { email, perfil });
        }

        [HttpGet]
        public IActionResult ConviteEnviado(string email, string perfil)
        {
            ViewBag.Email = email;
            ViewBag.Perfil = perfil;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Bloquear(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Erro"] = "Usuario invalido.";
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _userManager.FindByIdAsync(id);
            var usuarioAtual = await _userManager.GetUserAsync(User);

            if (usuario == null)
            {
                TempData["Erro"] = "Usuario nao encontrado.";
                return RedirectToAction(nameof(Index));
            }

            if (usuarioAtual != null && usuario.Id == usuarioAtual.Id)
            {
                TempData["Erro"] = "Voce nao pode bloquear seu proprio usuario.";
                return RedirectToAction(nameof(Index));
            }

            usuario.LockoutEnabled = true;
            usuario.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);

            var result = await _userManager.UpdateAsync(usuario);
            if (!result.Succeeded)
            {
                TempData["Erro"] = string.Join(" | ", result.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index));
            }

            TempData["Sucesso"] = "Usuario bloqueado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desbloquear(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Erro"] = "Usuario invalido.";
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                TempData["Erro"] = "Usuario nao encontrado.";
                return RedirectToAction(nameof(Index));
            }

            usuario.LockoutEnabled = true;
            usuario.LockoutEnd = null;

            var result = await _userManager.UpdateAsync(usuario);
            if (!result.Succeeded)
            {
                TempData["Erro"] = string.Join(" | ", result.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index));
            }

            TempData["Sucesso"] = "Usuario desbloqueado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Erro"] = "Usuario invalido.";
                return RedirectToAction(nameof(Index));
            }

            if (TentarObterIdConvite(id, out var conviteId))
            {
                var convite = await _identityContext.ConvitesUsuarios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == conviteId && c.AceitoEm == null);

                if (convite == null)
                {
                    TempData["Erro"] = "Convite nao encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                return View(new UsuarioListagemViewModel
                {
                    Id = id,
                    NomeCompleto = convite.NomeCompleto,
                    Email = convite.Email,
                    Perfil = convite.Perfil,
                    Status = "Pendente",
                    EmailConfirmado = false,
                    Bloqueado = false,
                    EhUsuarioAtual = false,
                    EhConvite = true
                });
            }

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                TempData["Erro"] = "Usuario nao encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var usuarioLogadoId = _userManager.GetUserId(User);
            var roles = await _userManager.GetRolesAsync(usuario);

            var model = new UsuarioListagemViewModel
            {
                Id = usuario.Id,
                NomeCompleto = string.IsNullOrWhiteSpace(usuario.NomeCompleto) ? "(Sem nome)" : usuario.NomeCompleto,
                Email = usuario.Email ?? "",
                EmailConfirmado = usuario.EmailConfirmed,
                Status = usuario.EmailConfirmed ? "Aceito" : "Pendente",
                Perfil = roles.Contains("Administrador") ? "Administrador"
                       : roles.Contains("Estudante") ? "Estudante"
                       : "Sem perfil",
                Bloqueado = usuario.LockoutEnd.HasValue && usuario.LockoutEnd.Value > DateTimeOffset.UtcNow,
                EhUsuarioAtual = usuarioLogadoId == usuario.Id,
                EhConvite = false
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Erro"] = "Usuario invalido.";
                return RedirectToAction(nameof(Index));
            }

            if (TentarObterIdConvite(id, out var conviteId))
            {
                var convite = await _identityContext.ConvitesUsuarios
                    .FirstOrDefaultAsync(c => c.Id == conviteId && c.AceitoEm == null);

                if (convite == null)
                {
                    TempData["Erro"] = "Convite nao encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                _identityContext.ConvitesUsuarios.Remove(convite);
                await _identityContext.SaveChangesAsync();
                TempData["Sucesso"] = "Convite excluido com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                TempData["Erro"] = "Usuario nao encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var usuarioLogadoId = _userManager.GetUserId(User);
            if (usuarioLogadoId == usuario.Id)
            {
                TempData["Erro"] = "Voce nao pode excluir seu proprio usuario.";
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(usuario);
            if (roles.Contains("Administrador"))
            {
                var administradores = await _userManager.GetUsersInRoleAsync("Administrador");
                if (administradores.Count <= 1)
                {
                    TempData["Erro"] = "Nao e possivel excluir o unico administrador do sistema.";
                    return RedirectToAction(nameof(Index));
                }
            }

            try
            {
                var result = await _userManager.DeleteAsync(usuario);

                if (!result.Succeeded)
                {
                    TempData["Erro"] = string.Join(" | ", result.Errors.Select(e => e.Description));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Nao foi possivel excluir este usuario. Detalhes: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Sucesso"] = "Usuario excluido com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        private static string GerarCodigoConvite()
        {
            return $"{Guid.NewGuid():N}{Guid.NewGuid():N}";
        }

        private static string MontarIdConvite(uint id)
        {
            return $"convite_{id}";
        }

        private static bool TentarObterIdConvite(string id, out uint conviteId)
        {
            conviteId = 0;

            return id.StartsWith("convite_", StringComparison.OrdinalIgnoreCase) &&
                   uint.TryParse(id["convite_".Length..], out conviteId);
        }
    }
}
