using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Service;
namespace BiotLabWeb.Controllers
{
    public class ExperimentoController : Controller
    {
        private readonly IExperimentoService experimentoService;
        private readonly IMapper mapper;

        public ExperimentoController(IExperimentoService experimentoService, IMapper mapper)
        {
            this.experimentoService = experimentoService;
            this.mapper = mapper;
        }

        // GET: ExperimentoController
        public ActionResult Index()
        {
            var experimento = experimentoService.GetAll();
            var vm = mapper.Map<IEnumerable<ExperimentoViewModel>>(experimento);
            return View(vm);
        }

        // GET: ExperimentoController/Details/5
        public ActionResult Details(uint id)
        {
            var experimento = experimentoService.Get(id);
            var vm = mapper.Map<ExperimentoViewModel>(experimento);
            return View(vm);
        }

        // GET: ExperimentoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExperimentoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExperimentoViewModel experimento)
        {
            try
            {
                var experimentoDB = mapper.Map<Experimento>(experimento);
                experimentoService.Create(experimentoDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExperimentoController/Edit/5
        public ActionResult Edit(uint id)
        {
            var experimento = experimentoService.Get(id);
            var vm = mapper.Map<ExperimentoViewModel>(experimento);
            return View(vm);
        }

        // POST: ExperimentoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ExperimentoViewModel experimento)
        {
            try
            {
                var experimentoDB = mapper.Map<Experimento>(experimento);
                experimentoService.Update(experimentoDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExperimentoController/Delete/5
        public ActionResult Delete(uint id)
        {
            var experimento = experimentoService.Get(id);
            var vm = mapper.Map<ExperimentoViewModel>(experimento);
            return View(vm);
        }

        // POST: ExperimentoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, ExperimentoViewModel experimento)
        {
            try
            {
                experimentoService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
