using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
namespace BiotLabWeb.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly IFornecedorService fornecedorService;
        private readonly IMapper mapper;

        public FornecedorController(IFornecedorService fornecedorService, IMapper mapper)
        {
            this.fornecedorService = fornecedorService;
            this.mapper = mapper;
        }

        // GET: FornecedorController
        public ActionResult Index()
        {
            var fornecedor = fornecedorService.GetAll();
            var vm = mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedor);
            return View(vm);
        }

        // GET: FornecedorController/Details/5
        public ActionResult Details(uint id)
        {
            var fornecedor = fornecedorService.Get(id);
            var vm = mapper.Map<FornecedorViewModel>(fornecedor);
            return View(vm);
        }

        // GET: FornecedorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FornecedorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FornecedorViewModel fornecedor)
        {
            try
            {
                var fornecedorDB = mapper.Map<Fornecedor>(fornecedor);
                fornecedorService.Create(fornecedorDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FornecedorController/Edit/5
        public ActionResult Edit(uint id)
        {
            var fornecedor = fornecedorService.Get(id);
            var vm = mapper.Map<FornecedorViewModel>(fornecedor);
            return View(vm);
        }

        // POST: FornecedorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FornecedorViewModel fornecedor)
        {
            try
            {
                var fornecedorDB = mapper.Map<Fornecedor>(fornecedor);
                fornecedorService.Update(fornecedorDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FornecedorController/Delete/5
        public ActionResult Delete(uint id)
        {
            var fornecedor = fornecedorService.Get(id);
            var vm = mapper.Map<FornecedorViewModel>(fornecedor);
            return View(vm);
        }

        // POST: FornecedorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, FornecedorViewModel fornecedor)
        {
            try
            {
                fornecedorService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
