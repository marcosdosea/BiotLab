using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Service;
namespace BiotLabWeb.Controllers
{
    public class ObituarioController : Controller
    {
        private readonly IObituarioService obituarioService;
        private readonly IMapper mapper;

        public ObituarioController(IObituarioService obituarioService, IMapper mapper)
        {
            this.obituarioService = obituarioService;
            this.mapper = mapper;
        }

        // GET: ObituarioController
        public ActionResult Index()
        {
            var obituario = obituarioService.GetAll();
            var vm = mapper.Map<IEnumerable<ObituarioViewModel>>(obituario);
            return View(vm);
        }

        // GET: ObtuarioController/Details/5
        public ActionResult Details(uint id)
        {
            var obituario = obituarioService.Get(id);
            var vm = mapper.Map<ObituarioViewModel>(obituario);
            return View(vm);
        }

        // GET: ObituarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ObituarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ObituarioViewModel obituario)
        {
            try
            {
                var obituarioDB = mapper.Map<Obituario>(obituario);
                obituarioService.Create(obituarioDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ObituarioController/Edit/5
        public ActionResult Edit(uint id)
        {
            var obituario = obituarioService.Get(id);
            var vm = mapper.Map<ObituarioViewModel>(obituario);
            return View(vm);
        }

        // POST: ObituarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ObituarioViewModel obituario)
        {
            try
            {
                var obituarioDB = mapper.Map<Obituario>(obituario);
                obituarioService.Update(obituarioDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ObituarioController/Delete/5
        public ActionResult Delete(uint id)
        {
            var obituario = obituarioService.Get(id);
            var vm = mapper.Map<ObituarioViewModel>(obituario);
            return View(vm);
        }

        // POST: ObituarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, ObituarioViewModel obituario)
        {
            try
            {
                obituarioService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
