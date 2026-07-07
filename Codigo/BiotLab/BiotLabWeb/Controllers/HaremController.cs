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
    public class HaremController : Controller
    {
        private readonly IHaremService haremService;
        private readonly IBioterioService bioterioService;
        private readonly IMapper mapper;

        public HaremController(IHaremService haremService, IBioterioService bioterioService, IMapper mapper)
        {
            this.haremService = haremService;
            this.bioterioService = bioterioService;
            this.mapper = mapper;
        }

        private void CarregarBioterios(uint? idBioterioSelecionado = null)
        {
            var bioterios = bioterioService.GetAll()
                .Select(b => new
                {
                    b.Id,
                    b.Nome
                })
                .ToList();

            ViewBag.Bioterios = new SelectList(bioterios, "Id", "Nome", idBioterioSelecionado);
        }

        public ActionResult Index()
        {
            var harems = haremService.GetAll().ToList();
            var bioterios = bioterioService.GetAll().ToList();

            var vm = harems.Select(h => new HaremViewModel
            {
                Id = h.Id,
                CodigoInterno = h.CodigoInterno,
                NumeroMachos = h.NumeroMachos,
                NumeroFemeas = h.NumeroFemeas,
                DataNascimento = h.DataNascimento,
                OrigemPai = h.OrigemPai,
                OrigemMae = h.OrigemMae,
                Status = h.Status,
                IdBioterio = h.IdBioterio,
                NomeBioterio = bioterios.FirstOrDefault(b => b.Id == h.IdBioterio)?.Nome
            }).ToList();

            return View(vm);
        }

        public ActionResult Details(uint id)
        {
            var harem = haremService.Get(id);
            if (harem == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<HaremViewModel>(harem);
            vm.NomeBioterio = bioterioService.Get(harem.IdBioterio)?.Nome;

            return View(vm);
        }

        public ActionResult Create()
        {
            CarregarBioterios();
            return View(new HaremViewModel
            {
                DataNascimento = DateTime.Today,
                Status = "A"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HaremViewModel harem)
        {
            if (!ModelState.IsValid)
            {
                CarregarBioterios(harem.IdBioterio);
                return View(harem);
            }

            var bioterio = bioterioService.Get(harem.IdBioterio);
            if (bioterio == null)
            {
                ModelState.AddModelError(nameof(harem.IdBioterio), "O biotério selecionado não existe.");
                CarregarBioterios(harem.IdBioterio);
                return View(harem);
            }

            try
            {
                var haremDB = mapper.Map<Harem>(harem);
                haremService.Create(haremDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível salvar o harém. {ex.InnerException?.Message ?? ex.Message}");
                CarregarBioterios(harem.IdBioterio);
                return View(harem);
            }
        }

        public ActionResult Edit(uint id)
        {
            var harem = haremService.Get(id);
            if (harem == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<HaremViewModel>(harem);
            CarregarBioterios(vm.IdBioterio);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, HaremViewModel harem)
        {
            if (id != harem.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                CarregarBioterios(harem.IdBioterio);
                return View(harem);
            }

            var atual = haremService.Get(id);
            if (atual == null)
            {
                return NotFound();
            }

            var bioterio = bioterioService.Get(harem.IdBioterio);
            if (bioterio == null)
            {
                ModelState.AddModelError(nameof(harem.IdBioterio), "O biotério selecionado não existe.");
                CarregarBioterios(harem.IdBioterio);
                return View(harem);
            }

            try
            {
                var haremDB = mapper.Map<Harem>(harem);
                haremService.Update(haremDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível atualizar o harém. {ex.InnerException?.Message ?? ex.Message}");
                CarregarBioterios(harem.IdBioterio);
                return View(harem);
            }
        }

        public ActionResult Delete(uint id)
        {
            var harem = haremService.Get(id);
            if (harem == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<HaremViewModel>(harem);
            vm.NomeBioterio = bioterioService.Get(harem.IdBioterio)?.Nome;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, HaremViewModel harem)
        {
            try
            {
                haremService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = haremService.Get(id);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<HaremViewModel>(existente);
                vm.NomeBioterio = bioterioService.Get(existente.IdBioterio)?.Nome;

                ModelState.AddModelError(string.Empty, $"Não foi possível excluir o harém. {ex.InnerException?.Message ?? ex.Message}");
                return View(vm);
            }
        }
    }
}
