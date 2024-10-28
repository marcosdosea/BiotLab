using AutoMapper;
using BiotLabWeb.Models;
using Core.Service;
using Core;
using Microsoft.AspNetCore.Mvc;
namespace BiotLabWeb.Controllers
{
    public class BioterioController : Controller
    {
        private readonly IBioterioService bioterioService;
        private readonly IMapper mapper;

        public BioterioController(IBioterioService bioterioService, IMapper mapper)
        {
            this.bioterioService = bioterioService;
            this.mapper = mapper;
        }

        // GET: BioterioController
        public ActionResult Index()
        {
            var bioterio = bioterioService.GetAll();
            var vm = mapper.Map<IEnumerable<BioterioViewModel>>(bioterio);
            return View(vm);
        }

        // GET: BioterioController/Details/5
        public ActionResult Details(uint id)
        {
            var bioterio = bioterioService.Get(id);
            var vm = mapper.Map<BioterioViewModel>(bioterio);
            return View(vm);
        }

        // GET: BioterioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BioterioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BioterioViewModel bioterio)
        {
            try
            {
                var bioterioDB = mapper.Map<Bioterio>(bioterio);
                bioterioService.Create(bioterioDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BioterioController/Edit/5
        public ActionResult Edit(uint id)
        {
            var bioterio = bioterioService.Get(id);
            var vm = mapper.Map<BioterioViewModel>(bioterio);
            return View(vm);
        }

        // POST: BioterioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BioterioViewModel bioterio)
        {
            try
            {
                var bioterioDB = mapper.Map<Bioterio>(bioterio);
                bioterioService.Update(bioterioDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BioterioController/Delete/5
        public ActionResult Delete(uint id)
        {
            var bioterio = bioterioService.Get(id);
            var vm = mapper.Map<BioterioViewModel>(bioterio);
            return View(vm);
        }

        // POST: BioterioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, BioterioViewModel bioterio)
        {
            try
            {
                bioterioService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
