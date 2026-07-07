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
    public class ExperimentoController : Controller
    {
        private readonly IExperimentoService experimentoService;
        private readonly IPesquisadorService pesquisadorService;
        private readonly IMapper mapper;

        public ExperimentoController(
            IExperimentoService experimentoService,
            IPesquisadorService pesquisadorService,
            IMapper mapper)
        {
            this.experimentoService = experimentoService;
            this.pesquisadorService = pesquisadorService;
            this.mapper = mapper;
        }

        private void CarregarPesquisadores(IEnumerable<uint>? idsPesquisadoresSelecionados = null)
        {
            ViewBag.PesquisadoresBloqueados ??= false;

            var pesquisadores = pesquisadorService.GetAll()
                .Select(p => new
                {
                    p.Id,
                    p.Nome
                })
                .ToList();

            ViewBag.Pesquisadores = new MultiSelectList(pesquisadores, "Id", "Nome", idsPesquisadoresSelecionados);
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

        private Pesquisador? VincularPesquisadorLogado(ExperimentoViewModel experimento)
        {
            ViewBag.PesquisadoresBloqueados = false;

            if (!DeveVincularPesquisadorLogado())
            {
                return null;
            }

            ViewBag.PesquisadoresBloqueados = true;

            var pesquisador = ObterPesquisadorDoUsuarioLogado();
            if (pesquisador == null)
            {
                ViewBag.PesquisadorLogadoNome = "Pesquisador não encontrado para o usuário logado";
                return null;
            }

            experimento.IdsPesquisadores = new List<uint> { pesquisador.Id };
            experimento.NomesPesquisadores = new List<string> { pesquisador.Nome };
            ViewBag.PesquisadorLogadoNome = pesquisador.Nome;
            ModelState.Remove(nameof(experimento.IdsPesquisadores));

            return pesquisador;
        }

        private void ValidarPesquisadoresSelecionados(ExperimentoViewModel experimento)
        {
            var ids = experimento.IdsPesquisadores
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (ids.Count == 0)
            {
                ModelState.AddModelError(nameof(experimento.IdsPesquisadores), "Selecione ao menos um pesquisador.");
                return;
            }

            foreach (var idPesquisador in ids)
            {
                if (pesquisadorService.Buscar(idPesquisador) == null)
                {
                    ModelState.AddModelError(nameof(experimento.IdsPesquisadores), "Um dos pesquisadores selecionados não existe.");
                    return;
                }
            }

            experimento.IdsPesquisadores = ids;
        }

        public ActionResult Index()
        {
            var vm = mapper.Map<List<ExperimentoViewModel>>(experimentoService.GetAll());
            return View(vm);
        }

        public ActionResult Details(uint id)
        {
            var experimento = experimentoService.Get(id);
            if (experimento == null)
            {
                return NotFound();
            }

            return View(mapper.Map<ExperimentoViewModel>(experimento));
        }

        public ActionResult Create()
        {
            var experimento = new ExperimentoViewModel
            {
                DataInicio = DateTime.Today,
                DataFim = DateTime.Today
            };

            VincularPesquisadorLogado(experimento);
            CarregarPesquisadores(experimento.IdsPesquisadores);

            return View(experimento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExperimentoViewModel experimento)
        {
            var pesquisadorLogado = VincularPesquisadorLogado(experimento);

            if (DeveVincularPesquisadorLogado() && pesquisadorLogado == null)
            {
                ModelState.AddModelError(
                    nameof(experimento.IdsPesquisadores),
                    "Não foi encontrado um pesquisador cadastrado com o e-mail do usuário logado.");
            }

            if (experimento.DataFim < experimento.DataInicio)
            {
                ModelState.AddModelError(nameof(experimento.DataFim), "A data de fim não pode ser menor que a data de início.");
            }

            if (DeveVincularPesquisadorLogado() && pesquisadorLogado == null)
            {
                ModelState.AddModelError(nameof(experimento.IdsPesquisadores), "Seu usuário não está vinculado a um pesquisador.");
            }

            ValidarPesquisadoresSelecionados(experimento);

            if (!ModelState.IsValid)
            {
                CarregarPesquisadores(experimento.IdsPesquisadores);
                return View(experimento);
            }

            try
            {
                var experimentoDB = mapper.Map<Experimento>(experimento);
                experimentoService.Create(experimentoDB, experimento.IdsPesquisadores);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível salvar o experimento. {ex.InnerException?.Message ?? ex.Message}");
                CarregarPesquisadores(experimento.IdsPesquisadores);
                return View(experimento);
            }
        }

        public ActionResult Edit(uint id)
        {
            var experimento = experimentoService.Get(id);
            if (experimento == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<ExperimentoViewModel>(experimento);
            VincularPesquisadorLogado(vm);
            CarregarPesquisadores(vm.IdsPesquisadores);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, ExperimentoViewModel experimento)
        {
            var pesquisadorLogado = VincularPesquisadorLogado(experimento);

            if (id != experimento.Id)
            {
                return BadRequest();
            }

            if (experimento.DataFim < experimento.DataInicio)
            {
                ModelState.AddModelError(nameof(experimento.DataFim), "A data de fim não pode ser menor que a data de início.");
            }

            ValidarPesquisadoresSelecionados(experimento);

            if (!ModelState.IsValid)
            {
                CarregarPesquisadores(experimento.IdsPesquisadores);
                return View(experimento);
            }

            var atual = experimentoService.Get(id);
            if (atual == null)
            {
                return NotFound();
            }

            try
            {
                var experimentoDB = mapper.Map<Experimento>(experimento);
                experimentoService.Update(experimentoDB, experimento.IdsPesquisadores);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível atualizar o experimento. {ex.InnerException?.Message ?? ex.Message}");
                CarregarPesquisadores(experimento.IdsPesquisadores);
                return View(experimento);
            }
        }

        public ActionResult Delete(uint id)
        {
            var experimento = experimentoService.Get(id);
            if (experimento == null)
            {
                return NotFound();
            }

            return View(mapper.Map<ExperimentoViewModel>(experimento));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, ExperimentoViewModel experimento)
        {
            ModelState.Clear();

            try
            {
                experimentoService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = experimentoService.Get(id);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<ExperimentoViewModel>(existente);
                var mensagem = ex is InvalidOperationException
                    ? ex.Message
                    : "Não foi possível excluir o experimento. Verifique se existem registros vinculados a ele.";

                ModelState.AddModelError(string.Empty, mensagem);
                return View(vm);
            }
        }
    }
}
