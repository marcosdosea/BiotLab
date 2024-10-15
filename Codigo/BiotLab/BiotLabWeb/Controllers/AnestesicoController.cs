using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace BiotLabWeb.Controllers
{
    public class AnestesicoController : Controller
    {
        private readonly IAnestesicoService anestesicoService;
        private readonly IMapper mapper;

        public AnestesicoController(IAnestesicoService anestesicoService, IMapper mapper)
        {
            this.anestesicoService = anestesicoService;
            this.mapper = mapper;
        }

        // GET: AnestesicoController
        public ActionResult Index()
        {
            var anestesicos = anestesicoService.GetAll();
            var vm = mapper.Map<IEnumerable<AnestesicoViewModel>>(anestesicos);
            return View(vm);
        }

        // GET: AnestesicoController/Details/5
        public ActionResult Details(uint id)
        {
            var anestesico = anestesicoService.Get(id);
            var vm = mapper.Map<AnestesicoViewModel>(anestesico);
            return View(vm);
        }

        // GET: AnestesicoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AnestesicoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnestesicoViewModel anestesico)
        {
            try
            {
                var anestesicoDB = mapper.Map<Anestesico>(anestesico);
                anestesicoService.Create(anestesicoDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AnestesicoController/Edit/5
        public ActionResult Edit(uint id)
        {
            var anestesico = anestesicoService.Get(id);
            var vm = mapper.Map<AnestesicoViewModel>(anestesico);
            return View(vm);
        }

        // POST: AnestesicoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, AnestesicoViewModel anestesico)
        {
            try
            {
                var anestesicoDB = mapper.Map<Anestesico>(anestesico);
                anestesicoService.Update(anestesicoDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AnestesicoController/Delete/5
        public ActionResult Delete(uint id)
        {
            var anestesico = anestesicoService.Get(id);
            var vm = mapper.Map<AnestesicoViewModel>(anestesico);
            return View(vm);
        }

        // POST: AnestesicoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, AnestesicoViewModel anestesico)
        {
            try
            {
                anestesicoService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
