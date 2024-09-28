using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models; // Presumindo que você tenha um namespace Models com a classe HarenViewModel

namespace BiotLabWeb.Controllers
{
    public class HarenController : Controller
    {
        private readonly IHarenService harenService;
        private readonly IMapper mapper;

        public HarenController(IHarenService harenService, IMapper mapper)
        {
            this.harenService = harenService;
            this.mapper = mapper;
        }

        // GET: HarenController
        public ActionResult Index()
        {
            var listaHarens = harenService.GetAll();
            var listaHarensModel = mapper.Map<List<HarenViewModel>>(listaHarens);
            return View(listaHarensModel);
        }

        // GET: HarenController/Details/5
        public ActionResult Details(int id)
        {
            var haren = harenService.Get(id);
            if (haren == null) return NotFound();

            HarenViewModel harenViewModel = mapper.Map<HarenViewModel>(haren);
            return View(harenViewModel);
        }

        // GET: HarenController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HarenController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HarenViewModel harenViewModel)
        {
            if (ModelState.IsValid)
            {
                var haren = mapper.Map<Haren>(harenViewModel);
                harenService.Create(haren);
                return RedirectToAction(nameof(Index));
            }
            return View(harenViewModel);
        }

        // GET: HarenController/Edit/5
        public ActionResult Edit(int id)
        {
            var haren = harenService.Get(id);
            if (haren == null) return NotFound();

            HarenViewModel harenViewModel = mapper.Map<HarenViewModel>(haren);
            return View(harenViewModel);
        }

        // POST: HarenController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HarenViewModel harenModel)
        {
            if (ModelState.IsValid)
            {
                var haren = mapper.Map<Haren>(harenModel);
                harenService.Edit(haren);
                return RedirectToAction(nameof(Index));
            }
            return View(harenModel);
        }

        // GET: HarenController/Delete/5
        public ActionResult Delete(int id)
        {
            var haren = harenService.Get(id);
            if (haren == null) return NotFound();

            HarenViewModel harenViewModel = mapper.Map<HarenViewModel>(haren);
            return View(harenViewModel);
        }

        // POST: HarenController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, HarenViewModel harenModel)
        {
            harenService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
