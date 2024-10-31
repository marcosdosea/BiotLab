using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace BiotLabWeb.Controllers
{
    public class GaiolaharemController : Controller
    {
        private readonly IGaiolaharemService gaiolaharemService;
        private readonly IGaiolaService gaiolaService;
        private readonly IHaremService haremService;
        private readonly IPesquisadorService pesquisadorService; // Certifique-se de declarar o campo
        private readonly IMapper mapper;

        public GaiolaharemController(
            IGaiolaharemService gaiolaharemService,
            IGaiolaService gaiolaService,
            IHaremService haremService,
            IPesquisadorService pesquisadorService,
            IMapper mapper)
        {
            this.gaiolaharemService = gaiolaharemService;
            this.gaiolaService = gaiolaService;
            this.haremService = haremService;
            this.pesquisadorService = pesquisadorService; // Atribuição adicionada
            this.mapper = mapper;
        }


        // GET: Gaiolaharem
        public ActionResult Index()
        {
            var gaiolaharems = gaiolaharemService.GetAll();
            var vm = mapper.Map<IEnumerable<GaiolaharemViewModel>>(gaiolaharems);
            return View(vm);
        }

        // GET: Gaiolaharem/Details/5/10
        public ActionResult Details(uint idGaiola, uint idHarem)
        {
            var gaiolaharem = gaiolaharemService.Get(idGaiola, idHarem);
            if (gaiolaharem == null)
                return NotFound();

            var vm = mapper.Map<GaiolaharemViewModel>(gaiolaharem);
            return View(vm);
        }

        // GET: Gaiolaharem/Create
        public ActionResult Create()
        {
            ViewBag.Gaiolas = GetGaiolaSelectList();
            ViewBag.Harems = GetHaremSelectList();
            ViewBag.Pesquisadores = GetPesquisadorSelectList();
            return View();
        }

        // POST: Gaiolaharem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GaiolaharemViewModel gaiolaharem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var gaiolaharemDomain = mapper.Map<Gaiolaharem>(gaiolaharem);
                    gaiolaharemService.Create(gaiolaharemDomain);
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Gaiolas = GetGaiolaSelectList();
                ViewBag.Harems = GetHaremSelectList();
                ViewBag.Pesquisadores = GetPesquisadorSelectList();
                return View(gaiolaharem);
            }
            catch
            {
                ViewBag.Gaiolas = GetGaiolaSelectList();
                ViewBag.Harems = GetHaremSelectList();
                ViewBag.Pesquisadores = GetPesquisadorSelectList();
                return View(gaiolaharem);
            }
        }

        // GET: Gaiolaharem/Edit/5/10
        public ActionResult Edit(uint idGaiola, uint idHarem)
        {
            var gaiolaharem = gaiolaharemService.Get(idGaiola, idHarem);
            if (gaiolaharem == null)
                return NotFound();

            var vm = mapper.Map<GaiolaharemViewModel>(gaiolaharem);
            ViewBag.Gaiolas = GetGaiolaSelectList();
            ViewBag.Harems = GetHaremSelectList();
            ViewBag.Pesquisadores = GetPesquisadorSelectList();
            return View(vm);
        }

        // POST: Gaiolaharem/Edit/5/10
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint idGaiola, uint idHarem, GaiolaharemViewModel gaiolaharem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var gaiolaharemDomain = mapper.Map<Gaiolaharem>(gaiolaharem);
                    gaiolaharemService.Update(gaiolaharemDomain);
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Gaiolas = GetGaiolaSelectList();
                ViewBag.Harems = GetHaremSelectList();
                ViewBag.Pesquisadores = GetPesquisadorSelectList();
                return View(gaiolaharem);
            }
            catch
            {
                ViewBag.Gaiolas = GetGaiolaSelectList();
                ViewBag.Harems = GetHaremSelectList();
                ViewBag.Pesquisadores = GetPesquisadorSelectList();
                return View(gaiolaharem);
            }
        }

        // GET: Gaiolaharem/Delete/5/10
        public ActionResult Delete(uint idGaiola, uint idHarem)
        {
            var gaiolaharem = gaiolaharemService.Get(idGaiola, idHarem);
            if (gaiolaharem == null)
                return NotFound();

            var vm = mapper.Map<GaiolaharemViewModel>(gaiolaharem);
            return View(vm);
        }

        // POST: Gaiolaharem/Delete/5/10
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(uint idGaiola, uint idHarem)
        {
            try
            {
                gaiolaharemService.Delete(idGaiola, idHarem);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Métodos auxiliares para preencher os dropdowns
        private IEnumerable<SelectListItem> GetGaiolaSelectList()
        {
            var gaiolas = gaiolaService.GetAll();
            var selectList = gaiolas.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Etiqueta
            });
            return selectList;
        }

        private IEnumerable<SelectListItem> GetHaremSelectList()
        {
            var harems = haremService.GetAll();
            var selectList = harems.Select(h => new SelectListItem
            {
                Value = h.Id.ToString(),
                Text = h.CodigoInterno
            });
            return selectList;
        }

        private IEnumerable<SelectListItem> GetPesquisadorSelectList()
        {
            var pesquisadores = pesquisadorService.GetAll();
            var selectList = pesquisadores.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nome
            });
            return selectList;
        }
    }

}
