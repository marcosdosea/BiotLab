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
    public class EntradaanestesicoController : Controller
    {
        private readonly IEntradaanestesicoService entradaAnestesicoService;
        private readonly IAnestesicosService anestesicoService;
        private readonly IEntradumService entradumService;
        private readonly IMapper mapper;

        public EntradaanestesicoController(
            IEntradaanestesicoService entradaAnestesicoService,
            IAnestesicosService anestesicoService,
            IEntradumService entradumService,
            IMapper mapper)
        {
            this.entradaAnestesicoService = entradaAnestesicoService;
            this.anestesicoService = anestesicoService;
            this.entradumService = entradumService;
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            var entradaAnestesicos = entradaAnestesicoService.GetAll();
            var vm = mapper.Map<IEnumerable<EntradaanestesicoViewModel>>(entradaAnestesicos);
            return View(vm);
        }

        public ActionResult Details(uint idEntrada, uint idAnestesico)
        {
            var entradaAnestesico = entradaAnestesicoService.Get(idEntrada, idAnestesico);
            if (entradaAnestesico == null)
                return NotFound();

            var vm = mapper.Map<EntradaanestesicoViewModel>(entradaAnestesico);
            return View(vm);
        }

        public ActionResult Create()
        {
            ViewBag.Anestesicos = GetAnestesicoSelectList();
            ViewBag.Entradas = GetEntradaSelectList();
            return View(new EntradaanestesicoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EntradaanestesicoViewModel entradaAnestesico)
        {
            if (entradaAnestesico.Quantidade <= 0)
            {
                ModelState.AddModelError(nameof(entradaAnestesico.Quantidade), "A quantidade deve ser maior que zero.");
            }

            if (entradaAnestesico.ValorUnitario <= 0)
            {
                ModelState.AddModelError(nameof(entradaAnestesico.ValorUnitario), "O valor unitário deve ser maior que zero.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Anestesicos = GetAnestesicoSelectList(entradaAnestesico.IdAnestesico);
                ViewBag.Entradas = GetEntradaSelectList(entradaAnestesico.IdEntrada);
                return View(entradaAnestesico);
            }

            if (anestesicoService.Buscar(entradaAnestesico.IdAnestesico) == null)
            {
                ModelState.AddModelError(nameof(entradaAnestesico.IdAnestesico), "O anestésico selecionado não existe.");
            }

            if (entradumService.Get(entradaAnestesico.IdEntrada) == null)
            {
                ModelState.AddModelError(nameof(entradaAnestesico.IdEntrada), "A entrada selecionada não existe.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Anestesicos = GetAnestesicoSelectList(entradaAnestesico.IdAnestesico);
                ViewBag.Entradas = GetEntradaSelectList(entradaAnestesico.IdEntrada);
                return View(entradaAnestesico);
            }

            try
            {
                var entradaAnestesicoDomain = mapper.Map<Entradaanestesico>(entradaAnestesico);
                entradaAnestesicoService.Create(entradaAnestesicoDomain);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível salvar a entrada de anestésico. {ex.InnerException?.Message ?? ex.Message}");
                ViewBag.Anestesicos = GetAnestesicoSelectList(entradaAnestesico.IdAnestesico);
                ViewBag.Entradas = GetEntradaSelectList(entradaAnestesico.IdEntrada);
                return View(entradaAnestesico);
            }
        }

        public ActionResult Edit(uint idEntrada, uint idAnestesico)
        {
            var entradaAnestesico = entradaAnestesicoService.Get(idEntrada, idAnestesico);
            if (entradaAnestesico == null)
                return NotFound();

            var vm = mapper.Map<EntradaanestesicoViewModel>(entradaAnestesico);
            ViewBag.Anestesicos = GetAnestesicoSelectList(vm.IdAnestesico);
            ViewBag.Entradas = GetEntradaSelectList(vm.IdEntrada);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint idEntrada, uint idAnestesico, EntradaanestesicoViewModel entradaAnestesico)
        {
            if (idEntrada != entradaAnestesico.IdEntrada || idAnestesico != entradaAnestesico.IdAnestesico)
            {
                return BadRequest();
            }

            if (entradaAnestesico.Quantidade <= 0)
            {
                ModelState.AddModelError(nameof(entradaAnestesico.Quantidade), "A quantidade deve ser maior que zero.");
            }

            if (entradaAnestesico.ValorUnitario <= 0)
            {
                ModelState.AddModelError(nameof(entradaAnestesico.ValorUnitario), "O valor unitário deve ser maior que zero.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Anestesicos = GetAnestesicoSelectList(entradaAnestesico.IdAnestesico);
                ViewBag.Entradas = GetEntradaSelectList(entradaAnestesico.IdEntrada);
                return View(entradaAnestesico);
            }

            var atual = entradaAnestesicoService.Get(idEntrada, idAnestesico);
            if (atual == null)
            {
                return NotFound();
            }

            try
            {
                var entradaAnestesicoDomain = mapper.Map<Entradaanestesico>(entradaAnestesico);
                entradaAnestesicoService.Update(entradaAnestesicoDomain);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível atualizar a entrada de anestésico. {ex.InnerException?.Message ?? ex.Message}");
                ViewBag.Anestesicos = GetAnestesicoSelectList(entradaAnestesico.IdAnestesico);
                ViewBag.Entradas = GetEntradaSelectList(entradaAnestesico.IdEntrada);
                return View(entradaAnestesico);
            }
        }

        public ActionResult Delete(uint idEntrada, uint idAnestesico)
        {
            var entradaAnestesico = entradaAnestesicoService.Get(idEntrada, idAnestesico);
            if (entradaAnestesico == null)
                return NotFound();

            var vm = mapper.Map<EntradaanestesicoViewModel>(entradaAnestesico);
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(uint idEntrada, uint idAnestesico)
        {
            try
            {
                entradaAnestesicoService.Delete(idEntrada, idAnestesico);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = entradaAnestesicoService.Get(idEntrada, idAnestesico);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<EntradaanestesicoViewModel>(existente);
                ModelState.AddModelError(string.Empty, $"Não foi possível excluir a entrada de anestésico. {ex.InnerException?.Message ?? ex.Message}");
                return View("Delete", vm);
            }
        }

        private IEnumerable<SelectListItem> GetAnestesicoSelectList(uint? selected = null)
        {
            var anestesicos = anestesicoService.GetAll();
            return anestesicos.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Nome,
                Selected = selected.HasValue && a.Id == selected.Value
            });
        }

        private IEnumerable<SelectListItem> GetEntradaSelectList(uint? selected = null)
        {
            var entradas = entradumService.GetAll();
            return entradas.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.NumeroNotaFiscal} - {e.DataEntrada:dd/MM/yyyy}",
                Selected = selected.HasValue && e.Id == selected.Value
            });
        }
    }
}