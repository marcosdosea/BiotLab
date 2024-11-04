using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using BiotLabWeb.Models;
using System.Globalization;

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
            var experimentosViewModel = _mapper.Map<List<ExperimentoViewModel>>(experimentos);
            return View(experimentosViewModel);
        }

        public IActionResult Create()
        {
            return View(new ExperimentoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExperimentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var experimento = _mapper.Map<Experimento>(model);
                experimento.DataInicio = DateTime.ParseExact(model.DataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                experimento.DataFim = DateTime.ParseExact(model.DataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                _experimentoService.Create(experimento);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Edit(uint id)
        {
            var experimento = _experimentoService.Get(id);
            if (experimento == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ExperimentoViewModel>(experimento);
            model.DataInicio = experimento.DataInicio.ToString("dd/MM/yyyy");
            model.DataFim = experimento.DataFim.ToString("dd/MM/yyyy");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(uint id, ExperimentoViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var experimento = _mapper.Map<Experimento>(model);
                experimento.DataInicio = DateTime.ParseExact(model.DataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                experimento.DataFim = DateTime.ParseExact(model.DataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                _experimentoService.Update(experimento);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Delete(uint id)
        {
            var experimento = _experimentoService.Get(id);
            if (experimento == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ExperimentoViewModel>(experimento);
            model.DataInicio = experimento.DataInicio.ToString("dd/MM/yyyy");
            model.DataFim = experimento.DataFim.ToString("dd/MM/yyyy");
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(uint id)
        {
            _experimentoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(uint id)
        {
            var experimento = _experimentoService.Get(id);
            if (experimento == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ExperimentoViewModel>(experimento);
            model.DataInicio = experimento.DataInicio.ToString("dd/MM/yyyy");
            model.DataFim = experimento.DataFim.ToString("dd/MM/yyyy");
            return View(model);
        }
    }
}
