using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BiotLabWeb.Controllers
{
    [Authorize(Roles = "Administrador,Estudante")]
    public class ObituarioController : Controller
    {
        private readonly IObituarioService obituarioService;
        private readonly IGaiolaService gaiolaService;
        private readonly IPesquisadorService pesquisadorService;
        private readonly IMapper mapper;

        public ObituarioController(
            IObituarioService obituarioService,
            IGaiolaService gaiolaService,
            IPesquisadorService pesquisadorService,
            IMapper mapper)
        {
            this.obituarioService = obituarioService;
            this.gaiolaService = gaiolaService;
            this.pesquisadorService = pesquisadorService;
            this.mapper = mapper;
        }

        private void CarregarCombos(uint? idGaiolaSelecionada = null, uint? idPesquisadorSelecionado = null)
        {
            var gaiolas = gaiolaService.GetAll()
                .Select(g => new
                {
                    g.Id,
                    Descricao = $"{g.CodigoInterno} - {g.Localizacao}"
                })
                .ToList();

            var pesquisadores = pesquisadorService.GetAll()
                .Select(p => new
                {
                    p.Id,
                    Descricao = p.Nome
                })
                .ToList();

            ViewBag.Gaiolas = new SelectList(gaiolas, "Id", "Descricao", idGaiolaSelecionada);
            ViewBag.Pesquisadores = new SelectList(pesquisadores, "Id", "Descricao", idPesquisadorSelecionado);
        }

        public ActionResult Index()
        {
            var obituarios = obituarioService.GetAll();
            var viewModel = mapper.Map<IEnumerable<ObituarioViewModel>>(obituarios);
            return View(viewModel);
        }

        public ActionResult Details(uint id)
        {
            var obituario = obituarioService.Buscar(id);
            if (obituario == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<ObituarioViewModel>(obituario);
            return View(vm);
        }

        public ActionResult Create()
        {
            CarregarCombos();
            return View(new ObituarioViewModel
            {
                Data = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ObituarioViewModel obituario)
        {
            if (!ModelState.IsValid)
            {
                CarregarCombos(obituario.IdGaiola, obituario.IdPesquisador);
                return View(obituario);
            }

            var gaiola = gaiolaService.Get(obituario.IdGaiola);
            if (gaiola == null)
            {
                ModelState.AddModelError(nameof(obituario.IdGaiola), "A gaiola selecionada não existe.");
            }

            var pesquisador = pesquisadorService.Buscar(obituario.IdPesquisador);
            if (pesquisador == null)
            {
                ModelState.AddModelError(nameof(obituario.IdPesquisador), "O pesquisador selecionado não existe.");
            }

            if (!ModelState.IsValid)
            {
                CarregarCombos(obituario.IdGaiola, obituario.IdPesquisador);
                return View(obituario);
            }

            try
            {
                var obituarioDB = mapper.Map<Obituario>(obituario);
                obituarioService.Create(obituarioDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Não foi possível salvar o registro de obituário.");
                CarregarCombos(obituario.IdGaiola, obituario.IdPesquisador);
                return View(obituario);
            }
        }

        public ActionResult Edit(uint id)
        {
            var obituario = obituarioService.Buscar(id);
            if (obituario == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<ObituarioViewModel>(obituario);
            CarregarCombos(vm.IdGaiola, vm.IdPesquisador);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, ObituarioViewModel obituario)
        {
            if (id != obituario.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                CarregarCombos(obituario.IdGaiola, obituario.IdPesquisador);
                return View(obituario);
            }

            var registroAtual = obituarioService.Buscar(id);
            if (registroAtual == null)
            {
                return NotFound();
            }

            var gaiola = gaiolaService.Get(obituario.IdGaiola);
            if (gaiola == null)
            {
                ModelState.AddModelError(nameof(obituario.IdGaiola), "A gaiola selecionada não existe.");
            }

            var pesquisador = pesquisadorService.Buscar(obituario.IdPesquisador);
            if (pesquisador == null)
            {
                ModelState.AddModelError(nameof(obituario.IdPesquisador), "O pesquisador selecionado não existe.");
            }

            if (!ModelState.IsValid)
            {
                CarregarCombos(obituario.IdGaiola, obituario.IdPesquisador);
                return View(obituario);
            }

            try
            {
                var obituarioDB = mapper.Map<Obituario>(obituario);
                obituarioService.Update(obituarioDB);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Não foi possível atualizar o registro de obituário.");
                CarregarCombos(obituario.IdGaiola, obituario.IdPesquisador);
                return View(obituario);
            }
        }

        public ActionResult Delete(uint id)
        {
            var obituario = obituarioService.Buscar(id);
            if (obituario == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<ObituarioViewModel>(obituario);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, ObituarioViewModel obituario)
        {
            try
            {
                obituarioService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = obituarioService.Buscar(id);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<ObituarioViewModel>(existente);
                ModelState.AddModelError(string.Empty, $"Não foi possível excluir o registro de obituário. {ex.InnerException?.Message ?? ex.Message}");
                return View(vm);
            }
        }
    }
}