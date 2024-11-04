using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BiotLabWeb.Controllers
{
    public class UsoanestesicoController : Controller
    {
        private readonly IUsoanestesicoService usoanestesicoService;
        private readonly IMapper mapper;

        public UsoanestesicoController(IUsoanestesicoService usoanestesicoService, IMapper mapper)
        {
            this.usoanestesicoService = usoanestesicoService;
            this.mapper = mapper;
        }

        // GET: UsoanestesicoController
        public ActionResult Index()
        {
            var usoanestesicos = usoanestesicoService.GetAll();
            var usoanestesicoViewModels = mapper.Map<List<UsoanestesicoViewModel>>(usoanestesicos);
            return View(usoanestesicoViewModels);
        }

        // GET: UsoanestesicoController/Details/5
        public ActionResult Details(uint id)
        {
            var usoanestesico = usoanestesicoService.Get(id);
            if (usoanestesico == null)
            {
                return NotFound();
            }
            var usoanestesicoViewModel = mapper.Map<UsoanestesicoViewModel>(usoanestesico);
            return View(usoanestesicoViewModel);
        }

        // GET: UsoanestesicoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsoanestesicoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsoanestesicoViewModel usoanestesico)
        {
            if (ModelState.IsValid)
            {
                var usoanestesicoEntity = mapper.Map<Usoanestesico>(usoanestesico);
                usoanestesicoService.Create(usoanestesicoEntity);
                return RedirectToAction(nameof(Index));
            }
            return View(usoanestesico);
        }

        // GET: UsoanestesicoController/Edit/5
        public ActionResult Edit(uint id)
        {
            var usoanestesico = usoanestesicoService.Get(id);
            if (usoanestesico == null)
            {
                return NotFound();
            }
            var usoanestesicoViewModel = mapper.Map<UsoanestesicoViewModel>(usoanestesico);
            return View(usoanestesicoViewModel);
        }

        // POST: UsoanestesicoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, UsoanestesicoViewModel usoanestesico)
        {
            if (ModelState.IsValid)
            {
                var usoanestesicoEntity = mapper.Map<Usoanestesico>(usoanestesico);
                usoanestesicoService.Update(usoanestesicoEntity);
                return RedirectToAction(nameof(Index));
            }
            return View(usoanestesico);
        }

        // GET: UsoanestesicoController/Delete/5
        public ActionResult Delete(uint id)
        {
            var usoanestesico = usoanestesicoService.Get(id);
            if (usoanestesico == null)
            {
                return NotFound();
            }
            var usoanestesicoViewModel = mapper.Map<UsoanestesicoViewModel>(usoanestesico);
            return View(usoanestesicoViewModel);
        }

        // POST: UsoanestesicoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, UsoanestesicoViewModel usoanestesico)
        {
            usoanestesicoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}