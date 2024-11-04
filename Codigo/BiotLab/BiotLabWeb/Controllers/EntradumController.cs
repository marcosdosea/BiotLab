using AutoMapper;
using BiotLabWeb.Models; 
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BiotLabWeb.Controllers
{
    public class EntradumController : Controller
    {
        private readonly IEntradumService entradumService; 
        private readonly IMapper mapper;

        public EntradumController(IEntradumService entradumService, IMapper mapper)
        {
            this.entradumService = entradumService;
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            var entradas = entradumService.GetAll(); 
            var entradaViewModels = mapper.Map<List<EntradumViewModel>>(entradas); 
            return View(entradaViewModels);
        }

      
        public ActionResult Details(uint id)
        {
            var entradum = entradumService.Get(id);
            if (entradum == null)
            {
                return NotFound();
            }
            var entradumViewModel = mapper.Map<EntradumViewModel>(entradum);
            return View(entradumViewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EntradumViewModel entradum)
        {
            if (ModelState.IsValid)
            {
                var entradumEntity = mapper.Map<Entradum>(entradum);
                entradumService.Create(entradumEntity);
                return RedirectToAction(nameof(Index));
            }
            return View(entradum);
        }

        public ActionResult Edit(uint id)
        {
            var entradum = entradumService.Get(id);
            if (entradum == null)
            {
                return NotFound();
            }
            var entradumViewModel = mapper.Map<EntradumViewModel>(entradum);
            return View(entradumViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, EntradumViewModel entradum)
        {
            if (ModelState.IsValid)
            {
                var entradumEntity = mapper.Map<Entradum>(entradum);
                entradumService.Update(entradumEntity);
                return RedirectToAction(nameof(Index));
            }
            return View(entradum);
        }

        public ActionResult Delete(uint id)
        {
            var entradum = entradumService.Get(id);
            if (entradum == null)
            {
                return NotFound();
            }
            var entradumViewModel = mapper.Map<EntradumViewModel>(entradum);
            return View(entradumViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, EntradumViewModel entradum)
        {
            entradumService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
