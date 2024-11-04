using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service;
using System.Collections.Generic;

namespace BiotLabWeb.Controllers
{
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

        // GET: Entradaanestesico
        public ActionResult Index()
        {
            var entradaAnestesicos = entradaAnestesicoService.GetAll();
            var vm = mapper.Map<IEnumerable<EntradaanestesicoViewModel>>(entradaAnestesicos);
            return View(vm);
        }

        // GET: Entradaanestesico/Details/5/10
        public ActionResult Details(uint idEntrada, uint idAnestesico)
        {
            var entradaAnestesico = entradaAnestesicoService.Get(idEntrada, idAnestesico);
            if (entradaAnestesico == null)
                return NotFound();

            var vm = mapper.Map<EntradaanestesicoViewModel>(entradaAnestesico);
            return View(vm);
        }

        // GET: Entradaanestesico/Create
        public ActionResult Create()
        {
            // Preencher listas para dropdowns, se necessário
            ViewBag.Anestesicos = GetAnestesicoSelectList();
            ViewBag.Entradas = GetEntradaSelectList();
            return View();
        }

        // POST: Entradaanestesico/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EntradaanestesicoViewModel entradaAnestesico)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entradaAnestesicoDomain = mapper.Map<Entradaanestesico>(entradaAnestesico);
                    entradaAnestesicoService.Create(entradaAnestesicoDomain);
                    return RedirectToAction(nameof(Index));
                }
                // Se o modelo não for válido, retornar as listas novamente
                ViewBag.Anestesicos = GetAnestesicoSelectList();
                ViewBag.Entradas = GetEntradaSelectList();
                return View(entradaAnestesico);
            }
            catch(Exception e)
            {
                // Em caso de erro, retornar as listas novamente
                ViewBag.Anestesicos = GetAnestesicoSelectList();
                ViewBag.Entradas = GetEntradaSelectList();
                return View(entradaAnestesico);
            }
        }

        // GET: Entradaanestesico/Edit/5/10
        public ActionResult Edit(uint idEntrada, uint idAnestesico)
        {
            var entradaAnestesico = entradaAnestesicoService.Get(idEntrada, idAnestesico);
            if (entradaAnestesico == null)
                return NotFound();

            var vm = mapper.Map<EntradaanestesicoViewModel>(entradaAnestesico);
            // Preencher listas para dropdowns, se necessário
            ViewBag.Anestesicos = GetAnestesicoSelectList();
            ViewBag.Entradas = GetEntradaSelectList();
            return View(vm);
        }

        // POST: Entradaanestesico/Edit/5/10
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint idEntrada, uint idAnestesico, EntradaanestesicoViewModel entradaAnestesico)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entradaAnestesicoDomain = mapper.Map<Entradaanestesico>(entradaAnestesico);
                    entradaAnestesicoService.Update(entradaAnestesicoDomain);
                    return RedirectToAction(nameof(Index));
                }
                // Se o modelo não for válido, retornar as listas novamente
                ViewBag.Anestesicos = GetAnestesicoSelectList();
                ViewBag.Entradas = GetEntradaSelectList();
                return View(entradaAnestesico);
            }
            catch
            {
                // Em caso de erro, retornar as listas novamente
                ViewBag.Anestesicos = GetAnestesicoSelectList();
                ViewBag.Entradas = GetEntradaSelectList();
                return View(entradaAnestesico);
            }
        }

        // GET: Entradaanestesico/Delete/5/10
        public ActionResult Delete(uint idEntrada, uint idAnestesico)
        {
            var entradaAnestesico = entradaAnestesicoService.Get(idEntrada, idAnestesico);
            if (entradaAnestesico == null)
                return NotFound();

            var vm = mapper.Map<EntradaanestesicoViewModel>(entradaAnestesico);
            return View(vm);
        }

        // POST: Entradaanestesico/Delete/5/10
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(uint idEntrada, uint idAnestesico)
        {
            try
            {
                entradaAnestesicoService.Delete(idEntrada, idAnestesico);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
        private IEnumerable<SelectListItem> GetAnestesicoSelectList()
        {
            var anestesicos = anestesicoService.GetAll();
            var selectList = anestesicos.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Nome
            });
            return selectList;
        }

        private IEnumerable<SelectListItem> GetEntradaSelectList()
        {
            var entradas = entradumService.GetAll();
            var selectList = entradas.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.DataEntrada.ToString("dd/MM/yyyy")
            });
            return selectList;
        }
    }
}
