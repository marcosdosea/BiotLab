using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BiotLabWeb.Controllers
{
    [Authorize(Roles = "PesquisadorSenior,Estudante")]
    public class AnestesicoController : Controller
    {
        private readonly IAnestesicosService anestesicoService;
        private readonly IInstituicaoService instituicaoService;
        private readonly IMapper mapper;

        public AnestesicoController(
            IAnestesicosService anestesicoService,
            IInstituicaoService instituicaoService,
            IMapper mapper)
        {
            this.anestesicoService = anestesicoService;
            this.instituicaoService = instituicaoService;
            this.mapper = mapper;
        }

        private void CarregarInstituicoes(uint? idInstituicaoSelecionada = null)
        {
            var instituicoes = instituicaoService.GetAll()
                .Select(i => new
                {
                    i.Id,
                    i.Nome
                })
                .ToList();

            ViewBag.Instituicoes = new SelectList(instituicoes, "Id", "Nome", idInstituicaoSelecionada);
        }

        public ActionResult Index()
        {
            var anestesicos = anestesicoService.GetAll().ToList();
            var instituicoes = instituicaoService.GetAll().ToList();

            var vm = anestesicos.Select(a => new AnestesicoViewModel
            {
                Id = a.Id,
                Nome = a.Nome,
                Marca = a.Marca,
                Concentracao = a.Concentracao,
                IdInstituicao = a.IdInstituicao,
                NomeInstituicao = instituicoes.FirstOrDefault(i => i.Id == a.IdInstituicao)?.Nome
            }).ToList();

            return View(vm);
        }

        public ActionResult Details(uint id)
        {
            var anestesico = anestesicoService.Buscar(id);
            if (anestesico == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<AnestesicoViewModel>(anestesico);

            var instituicao = instituicaoService.Get(anestesico.IdInstituicao);
            vm.NomeInstituicao = instituicao?.Nome;

            return View(vm);
        }

        public ActionResult Create()
        {
            CarregarInstituicoes();
            return View(new AnestesicoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnestesicoViewModel anestesico)
        {
            if (anestesico.Concentracao <= 0)
            {
                ModelState.AddModelError(nameof(anestesico.Concentracao), "Informe uma concentração válida.");
            }

            if (!ModelState.IsValid)
            {
                CarregarInstituicoes(anestesico.IdInstituicao);
                return View(anestesico);
            }

            var instituicao = instituicaoService.Get(anestesico.IdInstituicao);
            if (instituicao == null)
            {
                ModelState.AddModelError(nameof(anestesico.IdInstituicao), "A instituição selecionada não existe.");
                CarregarInstituicoes(anestesico.IdInstituicao);
                return View(anestesico);
            }

            try
            {
                var anestesicoDB = mapper.Map<Anestesico>(anestesico);
                anestesicoService.Create(anestesicoDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível salvar o anestésico. {ex.InnerException?.Message ?? ex.Message}");
                CarregarInstituicoes(anestesico.IdInstituicao);
                return View(anestesico);
            }
        }

        public ActionResult Edit(uint id)
        {
            var anestesico = anestesicoService.Buscar(id);
            if (anestesico == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<AnestesicoViewModel>(anestesico);
            CarregarInstituicoes(vm.IdInstituicao);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, AnestesicoViewModel anestesico)
        {
            if (id != anestesico.Id)
            {
                return BadRequest();
            }

            if (anestesico.Concentracao <= 0)
            {
                ModelState.AddModelError(nameof(anestesico.Concentracao), "Informe uma concentração válida.");
            }

            if (!ModelState.IsValid)
            {
                CarregarInstituicoes(anestesico.IdInstituicao);
                return View(anestesico);
            }

            var registroAtual = anestesicoService.Buscar(id);
            if (registroAtual == null)
            {
                return NotFound();
            }

            var instituicao = instituicaoService.Get(anestesico.IdInstituicao);
            if (instituicao == null)
            {
                ModelState.AddModelError(nameof(anestesico.IdInstituicao), "A instituição selecionada não existe.");
                CarregarInstituicoes(anestesico.IdInstituicao);
                return View(anestesico);
            }

            try
            {
                var anestesicoDB = mapper.Map<Anestesico>(anestesico);
                anestesicoService.Update(anestesicoDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível atualizar o anestésico. {ex.InnerException?.Message ?? ex.Message}");
                CarregarInstituicoes(anestesico.IdInstituicao);
                return View(anestesico);
            }
        }

        public ActionResult Delete(uint id)
        {
            var anestesico = anestesicoService.Buscar(id);
            if (anestesico == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<AnestesicoViewModel>(anestesico);

            var instituicao = instituicaoService.Get(anestesico.IdInstituicao);
            vm.NomeInstituicao = instituicao?.Nome;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, AnestesicoViewModel anestesico)
        {
            try
            {
                anestesicoService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = anestesicoService.Buscar(id);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<AnestesicoViewModel>(existente);
                var instituicao = instituicaoService.Get(existente.IdInstituicao);
                vm.NomeInstituicao = instituicao?.Nome;

                ModelState.AddModelError(string.Empty, $"Não foi possível excluir o anestésico. {ex.InnerException?.Message ?? ex.Message}");
                return View(vm);
            }
        }
    }
}
