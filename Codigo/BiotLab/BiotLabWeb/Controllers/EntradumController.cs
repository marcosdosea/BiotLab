using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BiotLabWeb.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EntradumController : Controller
    {
        private readonly IEntradumService entradumService;
        private readonly IFornecedorService fornecedorService;
        private readonly IInstituicaoService instituicaoService;
        private readonly IMapper mapper;

        public EntradumController(
            IEntradumService entradumService,
            IFornecedorService fornecedorService,
            IInstituicaoService instituicaoService,
            IMapper mapper)
        {
            this.entradumService = entradumService;
            this.fornecedorService = fornecedorService;
            this.instituicaoService = instituicaoService;
            this.mapper = mapper;
        }

        private void CarregarCombos(uint? idFornecedorSelecionado = null, uint? idInstituicaoSelecionada = null)
        {
            var fornecedores = fornecedorService.GetAll()
                .Select(f => new
                {
                    f.Id,
                    f.Nome
                })
                .ToList();

            var instituicoes = instituicaoService.GetAll()
                .Select(i => new
                {
                    i.Id,
                    i.Nome
                })
                .ToList();

            ViewBag.Fornecedores = new SelectList(fornecedores, "Id", "Nome", idFornecedorSelecionado);
            ViewBag.Instituicoes = new SelectList(instituicoes, "Id", "Nome", idInstituicaoSelecionada);
        }

        public ActionResult Index()
        {
            var entradas = entradumService.GetAll();
            var vm = mapper.Map<List<EntradumViewModel>>(entradas);
            return View(vm);
        }

        public ActionResult Details(uint id)
        {
            var entradum = entradumService.Get(id);
            if (entradum == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<EntradumViewModel>(entradum);
            return View(vm);
        }

        public ActionResult Create()
        {
            CarregarCombos();
            return View(new EntradumViewModel
            {
                DataEntrada = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EntradumViewModel entradum)
        {
            if (!ModelState.IsValid)
            {
                CarregarCombos(entradum.IdFornecedor, entradum.IdInstituicao);
                return View(entradum);
            }

            if (fornecedorService.Get(entradum.IdFornecedor) == null)
            {
                ModelState.AddModelError(nameof(entradum.IdFornecedor), "O fornecedor selecionado não existe.");
            }

            if (instituicaoService.Get(entradum.IdInstituicao) == null)
            {
                ModelState.AddModelError(nameof(entradum.IdInstituicao), "A instituição selecionada não existe.");
            }

            if (!ModelState.IsValid)
            {
                CarregarCombos(entradum.IdFornecedor, entradum.IdInstituicao);
                return View(entradum);
            }

            try
            {
                var entity = mapper.Map<Entradum>(entradum);
                entradumService.Create(entity);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível salvar a entrada. {ex.InnerException?.Message ?? ex.Message}");
                CarregarCombos(entradum.IdFornecedor, entradum.IdInstituicao);
                return View(entradum);
            }
        }

        public ActionResult Edit(uint id)
        {
            var entradum = entradumService.Get(id);
            if (entradum == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<EntradumViewModel>(entradum);
            CarregarCombos(vm.IdFornecedor, vm.IdInstituicao);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, EntradumViewModel entradum)
        {
            if (id != entradum.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                CarregarCombos(entradum.IdFornecedor, entradum.IdInstituicao);
                return View(entradum);
            }

            var atual = entradumService.Get(id);
            if (atual == null)
            {
                return NotFound();
            }

            if (fornecedorService.Get(entradum.IdFornecedor) == null)
            {
                ModelState.AddModelError(nameof(entradum.IdFornecedor), "O fornecedor selecionado não existe.");
            }

            if (instituicaoService.Get(entradum.IdInstituicao) == null)
            {
                ModelState.AddModelError(nameof(entradum.IdInstituicao), "A instituição selecionada não existe.");
            }

            if (!ModelState.IsValid)
            {
                CarregarCombos(entradum.IdFornecedor, entradum.IdInstituicao);
                return View(entradum);
            }

            try
            {
                var entity = mapper.Map<Entradum>(entradum);
                entradumService.Update(entity);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível atualizar a entrada. {ex.InnerException?.Message ?? ex.Message}");
                CarregarCombos(entradum.IdFornecedor, entradum.IdInstituicao);
                return View(entradum);
            }
        }

        public ActionResult Delete(uint id)
        {
            var entradum = entradumService.Get(id);
            if (entradum == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<EntradumViewModel>(entradum);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, EntradumViewModel entradum)
        {
            try
            {
                entradumService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = entradumService.Get(id);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<EntradumViewModel>(existente);
                ModelState.AddModelError(string.Empty, $"Não foi possível excluir a entrada. {ex.InnerException?.Message ?? ex.Message}");
                return View(vm);
            }
        }
    }
}