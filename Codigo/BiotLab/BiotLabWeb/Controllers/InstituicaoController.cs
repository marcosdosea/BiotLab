using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
namespace BiotLabWeb.Controllers
{
    public class InstituicaoController : Controller
    {
        private readonly IInstituicaoService instituicaoService;
        private readonly IMapper mapper;

        public InstituicaoController(IInstituicaoService instituicaoService, IMapper mapper)
        {
            this.instituicaoService = instituicaoService;
            this.mapper = mapper;
        }

        // GET: InstituicaoController
        public ActionResult Index()
        {
            var instituicao = instituicaoService.GetAll();
            var vm = mapper.Map<IEnumerable<InstituicaoViewModel>>(instituicao);
            return View(vm);
        }

        // GET: InstituicaoController/Details/5
        public ActionResult Details(uint id)
        {
            var instituicao = instituicaoService.Get(id);
            var vm = mapper.Map<InstituicaoViewModel>(instituicao);
            return View(vm);
        }

        // GET: InstituicaoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InstituicaoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstituicaoViewModel instituicao)
        {
            try
            {
                var instituicaoDB = mapper.Map<Instituicao>(instituicao);
                instituicaoService.Create(instituicaoDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InstituicaoController/Edit/5
        public ActionResult Edit(uint id)
        {
            var instituicao = instituicaoService.Get(id);
            var vm = mapper.Map<InstituicaoViewModel>(instituicao);
            return View(vm);
        }

        // POST: InstituicaoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InstituicaoViewModel instituicao)
        {
            try
            {
                var instituicaoDB = mapper.Map<Instituicao>(instituicao);
                instituicaoService.Update(instituicaoDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InstituicaoController/Delete/5
        public ActionResult Delete(uint id)
        {
            var instituicao = instituicaoService.Get(id);
            var vm = mapper.Map<InstituicaoViewModel>(instituicao);
            return View(vm);
        }

        // POST: InstituicaoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, InstituicaoViewModel instituicao)
        {
            try
            {
                instituicaoService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
