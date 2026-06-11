using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BiotLabWeb.Controllers
{
    [Authorize(Roles = "Administrador,Estudante")]
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

        private void CarregarPesquisadores(uint? idPesquisadorSelecionado = null)
        {
            var pesquisadores = pesquisadorService.GetAll()
                .Select(p => new
                {
                    p.Id,
                    p.Nome
                })
                .ToList();

            ViewBag.Pesquisadores = new SelectList(pesquisadores, "Id", "Nome", idPesquisadorSelecionado);
        }

        public ActionResult Index()
        {
            var experimentos = experimentoService.GetAll().ToList();
            var pesquisadores = pesquisadorService.GetAll().ToList();

            var vm = experimentos.Select(e => new ExperimentoViewModel
            {
                Id = e.Id,
                Cepa = e.Cepa,
                DataInicio = e.DataInicio,
                DataFim = e.DataFim,
                IdPesquisador = e.IdPesquisador,
                NomePesquisador = pesquisadores.FirstOrDefault(p => p.Id == e.IdPesquisador)?.Nome
            }).ToList();

            return View(vm);
        }

        public ActionResult Details(uint id)
        {
            var experimento = experimentoService.Get(id);
            if (experimento == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<ExperimentoViewModel>(experimento);
            vm.NomePesquisador = pesquisadorService.Buscar(experimento.IdPesquisador)?.Nome;

            return View(vm);
        }

        public ActionResult Create()
        {
            CarregarPesquisadores();
            return View(new ExperimentoViewModel
            {
                DataInicio = DateTime.Today,
                DataFim = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExperimentoViewModel experimento)
        {
            if (experimento.DataFim < experimento.DataInicio)
            {
                ModelState.AddModelError(nameof(experimento.DataFim), "A data de fim não pode ser menor que a data de início.");
            }

            if (!ModelState.IsValid)
            {
                CarregarPesquisadores(experimento.IdPesquisador);
                return View(experimento);
            }

            var pesquisador = pesquisadorService.Buscar(experimento.IdPesquisador);
            if (pesquisador == null)
            {
                ModelState.AddModelError(nameof(experimento.IdPesquisador), "O pesquisador selecionado não existe.");
                CarregarPesquisadores(experimento.IdPesquisador);
                return View(experimento);
            }

            try
            {
                var experimentoDB = mapper.Map<Experimento>(experimento);
                experimentoService.Create(experimentoDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível salvar o experimento. {ex.InnerException?.Message ?? ex.Message}");
                CarregarPesquisadores(experimento.IdPesquisador);
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
            CarregarPesquisadores(vm.IdPesquisador);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, ExperimentoViewModel experimento)
        {
            if (id != experimento.Id)
            {
                return BadRequest();
            }

            if (experimento.DataFim < experimento.DataInicio)
            {
                ModelState.AddModelError(nameof(experimento.DataFim), "A data de fim não pode ser menor que a data de início.");
            }

            if (!ModelState.IsValid)
            {
                CarregarPesquisadores(experimento.IdPesquisador);
                return View(experimento);
            }

            var atual = experimentoService.Get(id);
            if (atual == null)
            {
                return NotFound();
            }

            var pesquisador = pesquisadorService.Buscar(experimento.IdPesquisador);
            if (pesquisador == null)
            {
                ModelState.AddModelError(nameof(experimento.IdPesquisador), "O pesquisador selecionado não existe.");
                CarregarPesquisadores(experimento.IdPesquisador);
                return View(experimento);
            }

            try
            {
                var experimentoDB = mapper.Map<Experimento>(experimento);
                experimentoService.Update(experimentoDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível atualizar o experimento. {ex.InnerException?.Message ?? ex.Message}");
                CarregarPesquisadores(experimento.IdPesquisador);
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

            var vm = mapper.Map<ExperimentoViewModel>(experimento);
            vm.NomePesquisador = pesquisadorService.Buscar(experimento.IdPesquisador)?.Nome;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, ExperimentoViewModel experimento)
        {
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
                vm.NomePesquisador = pesquisadorService.Buscar(existente.IdPesquisador)?.Nome;

                ModelState.AddModelError(string.Empty, $"Não foi possível excluir o experimento. {ex.InnerException?.Message ?? ex.Message}");
                return View(vm);
            }
        }
    }
}