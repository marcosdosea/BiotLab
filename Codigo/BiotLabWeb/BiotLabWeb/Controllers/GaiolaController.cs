using AutoMapper;
using BiotLabWeb.Models;
using core;
using core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace BiotLabWeb.Controllers
{
    public class GaiolaController : Controller
    {
        private readonly IGaiolaService gaiolaService;
        private readonly IMapper mapper;

        public GaiolaController(IGaiolaService gaiolaService, IMapper mapper)
        {
            this.gaiolaService = gaiolaService;
            this.mapper = mapper;
        }

        // GET: GaiolaController
        public ActionResult Index()
        {
            var gaiolas = gaiolaService.GetAll();
            var vm = mapper.Map<IEnumerable<GaiolaViewModel>>(gaiolas);
            return View(vm);
        }

        // GET: GaiolaController/Details/5
        public ActionResult Details(int id)
        {
            var gaiola = gaiolaService.Get(id);
            var vm = mapper.Map<GaiolaViewModel>(gaiola);
            return View(vm);
        }

        // GET: GaiolaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GaiolaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GaiolaViewModel gaiola)
        {
            try
            {
                var gaiolaDB = mapper.Map<Gaiola>(gaiola);
                gaiolaService.Create(gaiolaDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GaiolaController/Edit/5
        public ActionResult Edit(int id)
        {
            var gaiola = gaiolaService.Get(id);
            var vm = mapper.Map<GaiolaViewModel>(gaiola);
            return View(vm);
        }

        // POST: GaiolaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GaiolaViewModel gaiola)
        {
            try
            {
                var gaiolaDB = mapper.Map<Gaiola>(gaiola);
                gaiolaService.Update(gaiolaDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GaiolaController/Delete/5
        public ActionResult Delete(int id)
        {
            var gaiola = gaiolaService.Get(id);
            var vm = mapper.Map<GaiolaViewModel>(gaiola);
            return View(vm);
        }

        // POST: GaiolaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, GaiolaViewModel gaiola)
        {
            try
            {
                gaiolaService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
