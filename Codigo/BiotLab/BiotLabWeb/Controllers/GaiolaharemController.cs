using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BiotLabWeb.Controllers
{
    [Authorize(Roles = "PesquisadorSenior,Estudante,Aluno")]
    public class GaiolaharemController : Controller
    {
        private readonly IGaiolaharemService gaiolaharemService;
        private readonly IGaiolaService gaiolaService;
        private readonly IHaremService haremService;
        private readonly IPesquisadorService pesquisadorService;
        private readonly IMapper mapper;

        public GaiolaharemController(
            IGaiolaharemService gaiolaharemService,
            IGaiolaService gaiolaService,
            IHaremService haremService,
            IPesquisadorService pesquisadorService,
            IMapper mapper)
        {
            this.gaiolaharemService = gaiolaharemService;
            this.gaiolaService = gaiolaService;
            this.haremService = haremService;
            this.pesquisadorService = pesquisadorService;
            this.mapper = mapper;
        }

        private bool DeveVincularPesquisadorLogado()
        {
            return User?.Identity?.IsAuthenticated == true && !User.IsInRole("PesquisadorSenior");
        }

        private Pesquisador? ObterPesquisadorDoUsuarioLogado()
        {
            var emailUsuario = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(emailUsuario))
            {
                return null;
            }

            return pesquisadorService.GetAll()
                .FirstOrDefault(p => string.Equals(p.Email, emailUsuario, StringComparison.OrdinalIgnoreCase));
        }

        private Pesquisador? VincularPesquisadorLogado(GaiolaharemViewModel gaiolaharem)
        {
            ViewBag.PesquisadorBloqueado = false;

            if (!DeveVincularPesquisadorLogado())
            {
                return null;
            }

            ViewBag.PesquisadorBloqueado = true;

            var pesquisador = ObterPesquisadorDoUsuarioLogado();
            if (pesquisador == null)
            {
                ViewBag.PesquisadorLogadoNome = "Pesquisador não encontrado para o usuário logado";
                return null;
            }

            gaiolaharem.IdPesquisador = pesquisador.Id;
            gaiolaharem.NomePesquisador = pesquisador.Nome;
            ViewBag.PesquisadorLogadoNome = pesquisador.Nome;
            ModelState.Remove(nameof(gaiolaharem.IdPesquisador));

            return pesquisador;
        }

        public ActionResult Index()
        {
            var gaiolaharems = gaiolaharemService.GetAll();
            var vm = mapper.Map<IEnumerable<GaiolaharemViewModel>>(gaiolaharems);
            return View(vm);
        }

        public ActionResult Details(uint idGaiola, uint idHarem)
        {
            var gaiolaharem = gaiolaharemService.Get(idGaiola, idHarem);
            if (gaiolaharem == null)
                return NotFound();

            var vm = mapper.Map<GaiolaharemViewModel>(gaiolaharem);
            return View(vm);
        }

        public ActionResult Create()
        {
            var vm = new GaiolaharemViewModel
            {
                DataPovoamento = DateTime.Today
            };

            VincularPesquisadorLogado(vm);
            ViewBag.Gaiolas = GetGaiolaSelectList();
            ViewBag.Harems = GetHaremSelectList();
            ViewBag.Pesquisadores = GetPesquisadorSelectList(vm.IdPesquisador);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GaiolaharemViewModel gaiolaharem)
        {
            var pesquisadorLogado = VincularPesquisadorLogado(gaiolaharem);

            if (DeveVincularPesquisadorLogado() && pesquisadorLogado == null)
            {
                ModelState.AddModelError(nameof(gaiolaharem.IdPesquisador), "Seu usuário não está vinculado a um pesquisador.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Gaiolas = GetGaiolaSelectList();
                ViewBag.Harems = GetHaremSelectList();
                ViewBag.Pesquisadores = GetPesquisadorSelectList(gaiolaharem.IdPesquisador);
                return View(gaiolaharem);
            }

            if (gaiolaService.Get(gaiolaharem.IdGaiola) == null)
            {
                ModelState.AddModelError(nameof(gaiolaharem.IdGaiola), "A gaiola selecionada nÃ£o existe.");
            }

            if (haremService.Get(gaiolaharem.IdHarem) == null)
            {
                ModelState.AddModelError(nameof(gaiolaharem.IdHarem), "O berçário selecionado não existe.");
            }

            if (pesquisadorService.Buscar(gaiolaharem.IdPesquisador) == null)
            {
                ModelState.AddModelError(nameof(gaiolaharem.IdPesquisador), "O pesquisador selecionado nÃ£o existe.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Gaiolas = GetGaiolaSelectList();
                ViewBag.Harems = GetHaremSelectList();
                ViewBag.Pesquisadores = GetPesquisadorSelectList(gaiolaharem.IdPesquisador);
                return View(gaiolaharem);
            }

            try
            {
                var gaiolaharemDomain = mapper.Map<Gaiolaharem>(gaiolaharem);
                gaiolaharemService.Create(gaiolaharemDomain);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"NÃ£o foi possÃ­vel salvar o vÃ­nculo. {ex.InnerException?.Message ?? ex.Message}");
                ViewBag.Gaiolas = GetGaiolaSelectList();
                ViewBag.Harems = GetHaremSelectList();
                ViewBag.Pesquisadores = GetPesquisadorSelectList(gaiolaharem.IdPesquisador);
                return View(gaiolaharem);
            }
        }

        public ActionResult Edit(uint idGaiola, uint idHarem)
        {
            var gaiolaharem = gaiolaharemService.Get(idGaiola, idHarem);
            if (gaiolaharem == null)
                return NotFound();

            var vm = mapper.Map<GaiolaharemViewModel>(gaiolaharem);
            VincularPesquisadorLogado(vm);
            ViewBag.Gaiolas = GetGaiolaSelectList();
            ViewBag.Harems = GetHaremSelectList();
            ViewBag.Pesquisadores = GetPesquisadorSelectList(vm.IdPesquisador);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint idGaiola, uint idHarem, GaiolaharemViewModel gaiolaharem)
        {
            var pesquisadorLogado = VincularPesquisadorLogado(gaiolaharem);

            if (idGaiola != gaiolaharem.IdGaiola || idHarem != gaiolaharem.IdHarem)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Gaiolas = GetGaiolaSelectList();
                ViewBag.Harems = GetHaremSelectList();
                ViewBag.Pesquisadores = GetPesquisadorSelectList(gaiolaharem.IdPesquisador);
                return View(gaiolaharem);
            }

            var atual = gaiolaharemService.Get(idGaiola, idHarem);
            if (atual == null)
            {
                return NotFound();
            }

            if (DeveVincularPesquisadorLogado() && pesquisadorLogado == null)
            {
                ModelState.AddModelError(nameof(gaiolaharem.IdPesquisador), "Seu usuário não está vinculado a um pesquisador.");
            }

            if (pesquisadorService.Buscar(gaiolaharem.IdPesquisador) == null)
            {
                ModelState.AddModelError(nameof(gaiolaharem.IdPesquisador), "O pesquisador selecionado nÃ£o existe.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Gaiolas = GetGaiolaSelectList();
                ViewBag.Harems = GetHaremSelectList();
                ViewBag.Pesquisadores = GetPesquisadorSelectList(gaiolaharem.IdPesquisador);
                return View(gaiolaharem);
            }

            try
            {
                var gaiolaharemDomain = mapper.Map<Gaiolaharem>(gaiolaharem);
                gaiolaharemService.Update(gaiolaharemDomain);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"NÃ£o foi possÃ­vel atualizar o vÃ­nculo. {ex.InnerException?.Message ?? ex.Message}");
                ViewBag.Gaiolas = GetGaiolaSelectList();
                ViewBag.Harems = GetHaremSelectList();
                ViewBag.Pesquisadores = GetPesquisadorSelectList(gaiolaharem.IdPesquisador);
                return View(gaiolaharem);
            }
        }

        public ActionResult Delete(uint idGaiola, uint idHarem)
        {
            var gaiolaharem = gaiolaharemService.Get(idGaiola, idHarem);
            if (gaiolaharem == null)
                return NotFound();

            var vm = mapper.Map<GaiolaharemViewModel>(gaiolaharem);
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(uint idGaiola, uint idHarem)
        {
            try
            {
                gaiolaharemService.Delete(idGaiola, idHarem);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = gaiolaharemService.Get(idGaiola, idHarem);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<GaiolaharemViewModel>(existente);
                ModelState.AddModelError(string.Empty, $"Não foi possível excluir o vínculo gaiola-berçário. {ex.InnerException?.Message ?? ex.Message}");
                return View("Delete", vm);
            }
        }

        private IEnumerable<SelectListItem> GetGaiolaSelectList(uint? selected = null)
        {
            var gaiolas = gaiolaService.GetAll();
            return gaiolas.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = string.IsNullOrWhiteSpace(g.CodigoInterno) ? g.Id.ToString() : g.CodigoInterno,
                Selected = selected.HasValue && g.Id == selected.Value
            });
        }

        private IEnumerable<SelectListItem> GetHaremSelectList(uint? selected = null)
        {
            var harems = haremService.GetAll();
            return harems.Select(h => new SelectListItem
            {
                Value = h.Id.ToString(),
                Text = string.IsNullOrWhiteSpace(h.CodigoInterno) ? h.Id.ToString() : h.CodigoInterno,
                Selected = selected.HasValue && h.Id == selected.Value
            });
        }

        private IEnumerable<SelectListItem> GetPesquisadorSelectList(uint? selected = null)
        {
            var pesquisadores = pesquisadorService.GetAll();
            return pesquisadores.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nome,
                Selected = selected.HasValue && p.Id == selected.Value
            });
        }
    }
}
