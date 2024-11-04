using AutoMapper;
using BiotLabWeb.Models;
using Core.Service;
using Core;
using Microsoft.AspNetCore.Mvc;

namespace BiotLabWeb.Controllers
{
    public class PesquisadorController : Controller
    {
        private readonly IPesquisadorService pesquisadorService;
        private readonly IMapper mapper;

        public PesquisadorController(IPesquisadorService pesquisadorService, IMapper mapper)
        {
            this.pesquisadorService = pesquisadorService;
            this.mapper = mapper;
        }

        // GET: PesquisadorController
        public ActionResult Index()
        {
            var pesquisadores = pesquisadorService.GetAll();
            var vm = mapper.Map<IEnumerable<PesquisadorViewModel>>(pesquisadores);
            return View(vm);
        }

        // GET: PesquisadorController/Details/5
        public ActionResult Details(uint id)
        {
            var pesquisador = pesquisadorService.Buscar(id);
            if (pesquisador == null)
            {
                return NotFound();
            }
            var vm = mapper.Map<PesquisadorViewModel>(pesquisador);
            return View(vm);
        }

        // GET: PesquisadorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PesquisadorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PesquisadorViewModel pesquisador)
        {
            try
            {
                var pesquisadorDB = mapper.Map<Pesquisador>(pesquisador);
                pesquisadorService.Create(pesquisadorDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PesquisadorController/Edit/5
        public ActionResult Edit(uint id)
        {
            var pesquisador = pesquisadorService.Buscar(id);
            if (pesquisador == null)
            {
                return NotFound();
            }
            var vm = mapper.Map<PesquisadorViewModel>(pesquisador);
            return View(vm);
        }

        // POST: PesquisadorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, PesquisadorViewModel pesquisador)
        {
            try
            {
                var pesquisadorDB = mapper.Map<Pesquisador>(pesquisador);
                pesquisadorService.Update(pesquisadorDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PesquisadorController/Delete/5
        public ActionResult Delete(uint id)
        {
            var pesquisador = pesquisadorService.Buscar(id);
            if (pesquisador == null)
            {
                return NotFound();
            }
            var vm = mapper.Map<PesquisadorViewModel>(pesquisador);
            return View(vm);
        }

        // POST: PesquisadorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, PesquisadorViewModel pesquisador)
        {
            try
            {
                pesquisadorService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}