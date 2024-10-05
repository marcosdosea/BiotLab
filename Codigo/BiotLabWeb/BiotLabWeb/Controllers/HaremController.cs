using AutoMapper;
using BiotLabWeb.Models;
using core;
using core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace BiotLabWeb.Controllers
{
    public class HaremController : Controller
    {
        private readonly IHaremService haremService;
        private readonly IMapper mapper;

        public HaremController(IHaremService haremService, IMapper mapper)
        {
            this.haremService = haremService;
            this.mapper = mapper;
        }

        // GET: HaremController
        public ActionResult Index()
        {
            var harens = haremService.GetAll();
            var vm = mapper.Map<IEnumerable<HaremViewModel>>(harens);
            return View(vm);
        }

        // GET: HaremController/Details/5
        public ActionResult Details(int id)
        {
            var harem = haremService.Get(id);
            var vm = mapper.Map<HaremViewModel>(harem);
            return View(vm);
        }

        // GET: HaremController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HaremController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HaremViewModel harem)
        {
            try
            {
                var haremDB = mapper.Map<Harem>(harem);
                haremService.Create(haremDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HaremController/Edit/5
        public ActionResult Edit(int id)
        {
            var harem = haremService.Get(id);
            var vm = mapper.Map<HaremViewModel>(harem);
            return View(vm);
        }

        // POST: HaremController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HaremViewModel harem)
        {
            try
            {
                var haremDB = mapper.Map<Harem>(harem);
                haremService.Update(haremDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HaremController/Delete/5
        public ActionResult Delete(int id)
        {
            var harem = haremService.Get(id);
            var vm = mapper.Map<HaremViewModel>(harem);
            return View(vm);
        }

        // POST: HaremController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, HaremViewModel harem)
        {
            try
            {
                haremService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
