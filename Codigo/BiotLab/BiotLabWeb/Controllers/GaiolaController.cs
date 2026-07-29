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
    public class GaiolaController : Controller
    {
        private readonly IGaiolaService gaiolaService;
        private readonly IBioterioService bioterioService;
        private readonly IExperimentoService experimentoService;
        private readonly IPesquisadorService pesquisadorService;
        private readonly IMapper mapper;

        public GaiolaController(
            IGaiolaService gaiolaService,
            IBioterioService bioterioService,
            IExperimentoService experimentoService,
            IPesquisadorService pesquisadorService,
            IMapper mapper)
        {
            this.gaiolaService = gaiolaService;
            this.bioterioService = bioterioService;
            this.experimentoService = experimentoService;
            this.pesquisadorService = pesquisadorService;
            this.mapper = mapper;
        }

        private void CarregarCombos(
            uint? idBioterioSelecionado = null,
            uint? idExperimentoSelecionado = null,
            uint? idPesquisadorSelecionado = null)
        {
            var bioterios = bioterioService.GetAll()
                .Select(b => new
                {
                    b.Id,
                    b.Nome
                })
                .ToList();

            var experimentos = experimentoService.GetAll()
                .Select(e => new
                {
                    e.Id,
                    Nome = $"{e.Titulo} ({e.DataInicio:dd/MM/yyyy} - {e.DataFim:dd/MM/yyyy})"
                })
                .ToList();

            var pesquisadores = pesquisadorService.GetAll()
                .Select(p => new
                {
                    p.Id,
                    p.Nome
                })
                .ToList();

            ViewBag.Bioterios = new SelectList(bioterios, "Id", "Nome", idBioterioSelecionado);
            ViewBag.Experimentos = new SelectList(experimentos, "Id", "Nome", idExperimentoSelecionado);
            ViewBag.Pesquisadores = new SelectList(pesquisadores, "Id", "Nome", idPesquisadorSelecionado);
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

        private Pesquisador? VincularPesquisadorLogado(GaiolaViewModel gaiola)
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

            gaiola.IdPesquisador = pesquisador.Id;
            gaiola.NomePesquisador = pesquisador.Nome;
            ViewBag.PesquisadorLogadoNome = pesquisador.Nome;
            ModelState.Remove(nameof(gaiola.IdPesquisador));

            return pesquisador;
        }

        public ActionResult Index()
        {
            var gaiolas = gaiolaService.GetAll().ToList();
            var bioterios = bioterioService.GetAll().ToList();
            var experimentos = experimentoService.GetAll().ToList();
            var pesquisadores = pesquisadorService.GetAll().ToList();

            var vm = gaiolas.Select(g => new GaiolaViewModel
            {
                Id = g.Id,
                CodigoInterno = g.CodigoInterno,
                NumeroMachos = g.NumeroMachos,
                NumeroFemeas = g.NumeroFemeas,
                Etiqueta = g.Etiqueta,
                Localizacao = g.Localizacao,
                Status = g.Status,
                IdBioterio = g.IdBioterio,
                IdExperimento = g.IdExperimento,
                IdPesquisador = g.IdPesquisador,
                NomeBioterio = bioterios.FirstOrDefault(b => b.Id == g.IdBioterio)?.Nome,
                NomeExperimento = g.IdExperimento.HasValue
                    ? experimentos.FirstOrDefault(e => e.Id == g.IdExperimento.Value)?.Titulo
                    : null,
                NomePesquisador = g.IdPesquisador.HasValue
                    ? pesquisadores.FirstOrDefault(p => p.Id == g.IdPesquisador.Value)?.Nome
                    : null
            }).ToList();

            return View(vm);
        }

        public ActionResult Details(uint id)
        {
            var gaiola = gaiolaService.Get(id);
            if (gaiola == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<GaiolaViewModel>(gaiola);
            vm.NomeBioterio = bioterioService.Get(gaiola.IdBioterio)?.Nome;
            vm.NomeExperimento = gaiola.IdExperimento.HasValue
                ? experimentoService.Get(gaiola.IdExperimento.Value)?.Titulo
                : null;
            vm.NomePesquisador = gaiola.IdPesquisador.HasValue
                ? pesquisadorService.Buscar(gaiola.IdPesquisador.Value)?.Nome
                : null;

            return View(vm);
        }

        public ActionResult Create()
        {
            var vm = new GaiolaViewModel
            {
                CodigoInterno = gaiolaService.GerarProximoCodigoInterno()
            };

            VincularPesquisadorLogado(vm);
            CarregarCombos(null, null, vm.IdPesquisador);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GaiolaViewModel gaiola)
        {
            ModelState.Remove(nameof(gaiola.CodigoInterno));
            var pesquisadorLogado = VincularPesquisadorLogado(gaiola);

            if (DeveVincularPesquisadorLogado() && pesquisadorLogado == null)
            {
                ModelState.AddModelError(nameof(gaiola.IdPesquisador), "Seu usuário não está vinculado a um pesquisador.");
            }

            if (!ModelState.IsValid)
            {
                gaiola.CodigoInterno = gaiolaService.GerarProximoCodigoInterno();
                CarregarCombos(gaiola.IdBioterio, gaiola.IdExperimento, gaiola.IdPesquisador);
                return View(gaiola);
            }

            if (bioterioService.Get(gaiola.IdBioterio) == null)
            {
                ModelState.AddModelError(nameof(gaiola.IdBioterio), "O biotério selecionado não existe.");
            }

            if (gaiola.IdExperimento.HasValue && experimentoService.Get(gaiola.IdExperimento.Value) == null)
            {
                ModelState.AddModelError(nameof(gaiola.IdExperimento), "O experimento selecionado não existe.");
            }

            if (gaiola.IdPesquisador.HasValue && pesquisadorService.Buscar(gaiola.IdPesquisador.Value) == null)
            {
                ModelState.AddModelError(nameof(gaiola.IdPesquisador), "O pesquisador selecionado não existe.");
            }

            if (!ModelState.IsValid)
            {
                gaiola.CodigoInterno = gaiolaService.GerarProximoCodigoInterno();
                CarregarCombos(gaiola.IdBioterio, gaiola.IdExperimento, gaiola.IdPesquisador);
                return View(gaiola);
            }

            try
            {
                var gaiolaDB = mapper.Map<Gaiola>(gaiola);
                gaiolaService.Create(gaiolaDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível salvar a gaiola. {ex.InnerException?.Message ?? ex.Message}");
                gaiola.CodigoInterno = gaiolaService.GerarProximoCodigoInterno();
                CarregarCombos(gaiola.IdBioterio, gaiola.IdExperimento, gaiola.IdPesquisador);
                return View(gaiola);
            }
        }

        public ActionResult Edit(uint id)
        {
            var gaiola = gaiolaService.Get(id);
            if (gaiola == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<GaiolaViewModel>(gaiola);
            VincularPesquisadorLogado(vm);
            CarregarCombos(vm.IdBioterio, vm.IdExperimento, vm.IdPesquisador);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, GaiolaViewModel gaiola)
        {
            ModelState.Remove(nameof(gaiola.CodigoInterno));
            var pesquisadorLogado = VincularPesquisadorLogado(gaiola);

            if (id != gaiola.Id)
            {
                return BadRequest();
            }

            var atual = gaiolaService.Get(id);
            if (atual == null)
            {
                return NotFound();
            }

            if (DeveVincularPesquisadorLogado() && pesquisadorLogado == null)
            {
                ModelState.AddModelError(nameof(gaiola.IdPesquisador), "Seu usuário não está vinculado a um pesquisador.");
            }

            if (!ModelState.IsValid)
            {
                gaiola.CodigoInterno = atual.CodigoInterno;
                CarregarCombos(gaiola.IdBioterio, gaiola.IdExperimento, gaiola.IdPesquisador);
                return View(gaiola);
            }

            if (bioterioService.Get(gaiola.IdBioterio) == null)
            {
                ModelState.AddModelError(nameof(gaiola.IdBioterio), "O biotério selecionado não existe.");
            }

            if (gaiola.IdExperimento.HasValue && experimentoService.Get(gaiola.IdExperimento.Value) == null)
            {
                ModelState.AddModelError(nameof(gaiola.IdExperimento), "O experimento selecionado não existe.");
            }

            if (gaiola.IdPesquisador.HasValue && pesquisadorService.Buscar(gaiola.IdPesquisador.Value) == null)
            {
                ModelState.AddModelError(nameof(gaiola.IdPesquisador), "O pesquisador selecionado não existe.");
            }

            if (!ModelState.IsValid)
            {
                gaiola.CodigoInterno = atual.CodigoInterno;
                CarregarCombos(gaiola.IdBioterio, gaiola.IdExperimento, gaiola.IdPesquisador);
                return View(gaiola);
            }

            try
            {
                gaiola.CodigoInterno = atual.CodigoInterno;
                var gaiolaDB = mapper.Map<Gaiola>(gaiola);
                gaiolaService.Update(gaiolaDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível atualizar a gaiola. {ex.InnerException?.Message ?? ex.Message}");
                gaiola.CodigoInterno = atual.CodigoInterno;
                CarregarCombos(gaiola.IdBioterio, gaiola.IdExperimento, gaiola.IdPesquisador);
                return View(gaiola);
            }
        }

        public ActionResult Delete(uint id)
        {
            var gaiola = gaiolaService.Get(id);
            if (gaiola == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<GaiolaViewModel>(gaiola);
            vm.NomeBioterio = bioterioService.Get(gaiola.IdBioterio)?.Nome;
            vm.NomeExperimento = gaiola.IdExperimento.HasValue
                ? experimentoService.Get(gaiola.IdExperimento.Value)?.Titulo
                : null;
            vm.NomePesquisador = gaiola.IdPesquisador.HasValue
                ? pesquisadorService.Buscar(gaiola.IdPesquisador.Value)?.Nome
                : null;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, GaiolaViewModel gaiola)
        {
            try
            {
                gaiolaService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = gaiolaService.Get(id);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<GaiolaViewModel>(existente);
                vm.NomeBioterio = bioterioService.Get(existente.IdBioterio)?.Nome;
                vm.NomeExperimento = existente.IdExperimento.HasValue
                    ? experimentoService.Get(existente.IdExperimento.Value)?.Titulo
                    : null;
                vm.NomePesquisador = existente.IdPesquisador.HasValue
                    ? pesquisadorService.Buscar(existente.IdPesquisador.Value)?.Nome
                    : null;

                ModelState.AddModelError(string.Empty, $"Não foi possível excluir a gaiola. {ex.InnerException?.Message ?? ex.Message}");
                return View(vm);
            }
        }
    }
}
