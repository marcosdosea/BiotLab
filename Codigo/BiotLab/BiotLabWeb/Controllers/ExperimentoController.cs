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
        private readonly IExperimentoService _experimentoService;
        private readonly IMapper _mapper;

        public ExperimentoController(IExperimentoService experimentoService, IMapper mapper)
        {
            _experimentoService = experimentoService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var experimentos = _experimentoService.GetAll();
            var model = _mapper.Map<List<ExperimentoViewModel>>(experimentos);
            return View(model);
        }

        public IActionResult Details(uint id)
        {
            var experimento = _experimentoService.Get(id);
            if (experimento == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ExperimentoViewModel>(experimento);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ExperimentoViewModel());
        }

        [HttpPost]
        public IActionResult Create(ExperimentoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var experimento = _mapper.Map<Experimento>(model);
            _experimentoService.Create(experimento);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(uint id)
        {
            var experimento = _experimentoService.Get(id);
            if (experimento == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ExperimentoViewModel>(experimento);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(uint id, ExperimentoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var experimento = _mapper.Map<Experimento>(model);
            _experimentoService.Update(experimento);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(uint id)
        {
            var experimento = _experimentoService.Get(id);
            if (experimento == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ExperimentoViewModel>(experimento);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(uint id, ExperimentoViewModel model)
        {
            _experimentoService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
