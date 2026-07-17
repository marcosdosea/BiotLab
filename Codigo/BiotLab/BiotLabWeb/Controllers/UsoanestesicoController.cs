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
    [Authorize(Roles = "Administrador,Estudante,Aluno")]
    public class UsoanestesicoController : Controller
    {
        private readonly IUsoanestesicoService usoanestesicoService;
        private readonly IPesquisadorService pesquisadorService;
        private readonly IExperimentoService experimentoService;
        private readonly IEntradaanestesicoService entradaanestesicoService;
        private readonly IMapper mapper;

        public UsoanestesicoController(
            IUsoanestesicoService usoanestesicoService,
            IPesquisadorService pesquisadorService,
            IExperimentoService experimentoService,
            IEntradaanestesicoService entradaanestesicoService,
            IMapper mapper)
        {
            this.usoanestesicoService = usoanestesicoService;
            this.pesquisadorService = pesquisadorService;
            this.experimentoService = experimentoService;
            this.entradaanestesicoService = entradaanestesicoService;
            this.mapper = mapper;
        }

        private void CarregarCombos(
            uint? idPesquisadorSelecionado = null,
            uint? idExperimentoSelecionado = null,
            uint? idEntradaSelecionada = null,
            uint? idAnestesicoSelecionado = null)
        {
            var pesquisadores = pesquisadorService.GetAll()
                .Select(p => new
                {
                    p.Id,
                    p.Nome
                })
                .ToList();

            var experimentos = experimentoService.GetAll()
                .Select(e => new
                {
                    e.Id,
                    Nome = $"{e.Titulo} ({e.DataInicio:dd/MM/yyyy} - {e.DataFim:dd/MM/yyyy})"
                })
                .ToList();

            var entradas = entradaanestesicoService.GetAll()
                .Select(e => new
                {
                    e.IdEntrada,
                    Texto = $"{e.IdEntradaNavigation.NumeroNotaFiscal} - {e.IdEntradaNavigation.DataEntrada:dd/MM/yyyy}"
                })
                .Distinct()
                .OrderBy(e => e.Texto)
                .ToList();

            var anestesicos = entradaanestesicoService.GetAll()
                .Select(e => new
                {
                    e.IdAnestesico,
                    Texto = $"{e.IdAnestesicoNavigation.Nome} - Lote {e.Lote}"
                })
                .Distinct()
                .OrderBy(e => e.Texto)
                .ToList();

            ViewBag.Pesquisadores = new SelectList(pesquisadores, "Id", "Nome", idPesquisadorSelecionado);
            ViewBag.Experimentos = new SelectList(experimentos, "Id", "Nome", idExperimentoSelecionado);
            ViewBag.Entradas = new SelectList(entradas, "IdEntrada", "Texto", idEntradaSelecionada);
            ViewBag.Anestesicos = new SelectList(anestesicos, "IdAnestesico", "Texto", idAnestesicoSelecionado);
        }

        private bool DeveVincularPesquisadorLogado()
        {
            return User?.Identity?.IsAuthenticated == true && !User.IsInRole("Administrador");
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

        private Pesquisador? VincularPesquisadorLogado(UsoanestesicoViewModel usoanestesico)
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

            usoanestesico.IdPesquisador = pesquisador.Id;
            usoanestesico.NomePesquisador = pesquisador.Nome;
            ViewBag.PesquisadorLogadoNome = pesquisador.Nome;
            ModelState.Remove(nameof(usoanestesico.IdPesquisador));

            return pesquisador;
        }

        public ActionResult Index()
        {
            var usoanestesicos = usoanestesicoService.GetAll();
            var vm = mapper.Map<List<UsoanestesicoViewModel>>(usoanestesicos);
            return View(vm);
        }

        public ActionResult Details(uint id)
        {
            var usoanestesico = usoanestesicoService.Get(id);
            if (usoanestesico == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<UsoanestesicoViewModel>(usoanestesico);
            return View(vm);
        }

        public ActionResult Create()
        {
            var usoanestesico = new UsoanestesicoViewModel
            {
                Data = DateTime.Today
            };

            VincularPesquisadorLogado(usoanestesico);
            CarregarCombos(usoanestesico.IdPesquisador);

            return View(usoanestesico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsoanestesicoViewModel usoanestesico)
        {
            var pesquisadorLogado = VincularPesquisadorLogado(usoanestesico);

            if (DeveVincularPesquisadorLogado() && pesquisadorLogado == null)
            {
                ModelState.AddModelError(
                    nameof(usoanestesico.IdPesquisador),
                    "Não foi encontrado um pesquisador cadastrado com o e-mail do usuário logado.");
            }

            if (usoanestesico.Quantidade <= 0)
            {
                ModelState.AddModelError(nameof(usoanestesico.Quantidade), "A quantidade deve ser maior que zero.");
            }

            if (DeveVincularPesquisadorLogado() && pesquisadorLogado == null)
            {
                ModelState.AddModelError(nameof(usoanestesico.IdPesquisador), "Seu usuário não está vinculado a um pesquisador.");
            }

            if (!ModelState.IsValid)
            {
                CarregarCombos(
                    usoanestesico.IdPesquisador,
                    usoanestesico.IdExperimento,
                    usoanestesico.IdEntrada,
                    usoanestesico.IdAnestesico);
                return View(usoanestesico);
            }

            if (pesquisadorService.Buscar(usoanestesico.IdPesquisador) == null)
            {
                ModelState.AddModelError(nameof(usoanestesico.IdPesquisador), "O pesquisador selecionado não existe.");
            }

            if (experimentoService.Get(usoanestesico.IdExperimento) == null)
            {
                ModelState.AddModelError(nameof(usoanestesico.IdExperimento), "O experimento selecionado não existe.");
            }

            if (entradaanestesicoService.Get(usoanestesico.IdEntrada, usoanestesico.IdAnestesico) == null)
            {
                ModelState.AddModelError(string.Empty, "A combinação de entrada e anestésico selecionada não existe em Entrada de Anestésico.");
            }

            if (!ModelState.IsValid)
            {
                CarregarCombos(
                    usoanestesico.IdPesquisador,
                    usoanestesico.IdExperimento,
                    usoanestesico.IdEntrada,
                    usoanestesico.IdAnestesico);
                return View(usoanestesico);
            }

            try
            {
                var entity = mapper.Map<Usoanestesico>(usoanestesico);
                usoanestesicoService.Create(entity);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível salvar o uso de anestésico. {ex.InnerException?.Message ?? ex.Message}");
                CarregarCombos(
                    usoanestesico.IdPesquisador,
                    usoanestesico.IdExperimento,
                    usoanestesico.IdEntrada,
                    usoanestesico.IdAnestesico);
                return View(usoanestesico);
            }
        }

        public ActionResult Edit(uint id)
        {
            var usoanestesico = usoanestesicoService.Get(id);
            if (usoanestesico == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<UsoanestesicoViewModel>(usoanestesico);
            VincularPesquisadorLogado(vm);
            CarregarCombos(vm.IdPesquisador, vm.IdExperimento, vm.IdEntrada, vm.IdAnestesico);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, UsoanestesicoViewModel usoanestesico)
        {
            var pesquisadorLogado = VincularPesquisadorLogado(usoanestesico);

            if (id != usoanestesico.Id)
            {
                return BadRequest();
            }

            if (usoanestesico.Quantidade <= 0)
            {
                ModelState.AddModelError(nameof(usoanestesico.Quantidade), "A quantidade deve ser maior que zero.");
            }

            if (!ModelState.IsValid)
            {
                CarregarCombos(
                    usoanestesico.IdPesquisador,
                    usoanestesico.IdExperimento,
                    usoanestesico.IdEntrada,
                    usoanestesico.IdAnestesico);
                return View(usoanestesico);
            }

            var atual = usoanestesicoService.Get(id);
            if (atual == null)
            {
                return NotFound();
            }

            if (pesquisadorService.Buscar(usoanestesico.IdPesquisador) == null)
            {
                ModelState.AddModelError(nameof(usoanestesico.IdPesquisador), "O pesquisador selecionado não existe.");
            }

            if (experimentoService.Get(usoanestesico.IdExperimento) == null)
            {
                ModelState.AddModelError(nameof(usoanestesico.IdExperimento), "O experimento selecionado não existe.");
            }

            if (entradaanestesicoService.Get(usoanestesico.IdEntrada, usoanestesico.IdAnestesico) == null)
            {
                ModelState.AddModelError(string.Empty, "A combinação de entrada e anestésico selecionada não existe em Entrada de Anestésico.");
            }

            if (!ModelState.IsValid)
            {
                CarregarCombos(
                    usoanestesico.IdPesquisador,
                    usoanestesico.IdExperimento,
                    usoanestesico.IdEntrada,
                    usoanestesico.IdAnestesico);
                return View(usoanestesico);
            }

            try
            {
                var entity = mapper.Map<Usoanestesico>(usoanestesico);
                usoanestesicoService.Update(entity);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível atualizar o uso de anestésico. {ex.InnerException?.Message ?? ex.Message}");
                CarregarCombos(
                    usoanestesico.IdPesquisador,
                    usoanestesico.IdExperimento,
                    usoanestesico.IdEntrada,
                    usoanestesico.IdAnestesico);
                return View(usoanestesico);
            }
        }

        public ActionResult Delete(uint id)
        {
            var usoanestesico = usoanestesicoService.Get(id);
            if (usoanestesico == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<UsoanestesicoViewModel>(usoanestesico);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, UsoanestesicoViewModel usoanestesico)
        {
            try
            {
                usoanestesicoService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = usoanestesicoService.Get(id);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<UsoanestesicoViewModel>(existente);
                ModelState.AddModelError(string.Empty, $"Não foi possível excluir o uso de anestésico. {ex.InnerException?.Message ?? ex.Message}");
                return View(vm);
            }
        }
    }
}
