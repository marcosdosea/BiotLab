using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;


namespace BiotLabWeb.Controllers
{
    [Authorize(Roles = "PesquisadorSenior")]
    public class BioterioController : Controller
    {
        private readonly IBioterioService bioterioService;
        private readonly IInstituicaoService instituicaoService;
        private readonly IMapper mapper;

        public BioterioController(IBioterioService bioterioService, IInstituicaoService instituicaoService, IMapper mapper)
        {
            this.bioterioService = bioterioService;
            this.instituicaoService = instituicaoService;
            this.mapper = mapper;
        }

        private void CarregarInstituicoes(uint? idInstituicaoSelecionada = null)
        {
            var instituicoes = instituicaoService.GetAll()
                .Select(i => new
                {
                    i.Id,
                    i.Nome
                })
                .ToList();

            ViewBag.Instituicoes = new SelectList(instituicoes, "Id", "Nome", idInstituicaoSelecionada);
        }

        private static string SomenteDigitos(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            return Regex.Replace(valor, @"\D", "");
        }

        private static string? LimparTextoOpcional(string? valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
        }

        private static void NormalizarBioterioViewModel(BioterioViewModel bioterio)
        {
            bioterio.Nome = bioterio.Nome?.Trim() ?? string.Empty;
            bioterio.Cep = SomenteDigitos(bioterio.Cep);
            bioterio.Rua = LimparTextoOpcional(bioterio.Rua);
            bioterio.Bairro = LimparTextoOpcional(bioterio.Bairro);
            bioterio.Cidade = LimparTextoOpcional(bioterio.Cidade);
            bioterio.Numero = LimparTextoOpcional(bioterio.Numero);
            bioterio.Complemento = LimparTextoOpcional(bioterio.Complemento);
            bioterio.Estado = (bioterio.Estado ?? string.Empty).Trim().ToUpper();
            bioterio.Telefone1 = SomenteDigitos(bioterio.Telefone1);
            bioterio.Telefone2 = string.IsNullOrWhiteSpace(bioterio.Telefone2)
                ? null
                : SomenteDigitos(bioterio.Telefone2);
            bioterio.Email = bioterio.Email?.Trim() ?? string.Empty;
        }

        private static void ValidarCamposNormalizados(BioterioViewModel bioterio, ModelStateDictionary modelState)
        {
            if (bioterio.Cep.Length != 8)
            {
                modelState.AddModelError(nameof(bioterio.Cep), "O CEP deve conter 8 dígitos.");
            }

            if (bioterio.Estado.Length != 2)
            {
                modelState.AddModelError(nameof(bioterio.Estado), "O estado deve conter 2 caracteres.");
            }
        }

        public ActionResult Index()
        {
            var bioterios = bioterioService.GetAll().ToList();
            var instituicoes = instituicaoService.GetAll().ToList();

            var vm = bioterios.Select(b => new BioterioViewModel
            {
                Id = b.Id,
                Nome = b.Nome,
                Cep = b.Cep,
                Rua = b.Rua,
                Bairro = b.Bairro,
                Cidade = b.Cidade,
                Numero = b.Numero,
                Complemento = b.Complemento,
                Estado = b.Estado,
                Telefone1 = b.Telefone1,
                Telefone2 = b.Telefone2,
                Email = b.Email,
                IdInstituicao = b.IdInstituicao,
                NomeInstituicao = instituicoes.FirstOrDefault(i => i.Id == b.IdInstituicao)?.Nome
            }).ToList();

            return View(vm);
        }

        public ActionResult Details(uint id)
        {
            var bioterio = bioterioService.Get(id);
            if (bioterio == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<BioterioViewModel>(bioterio);

            var instituicao = instituicaoService.Get(bioterio.IdInstituicao);
            vm.NomeInstituicao = instituicao?.Nome;

            return View(vm);
        }

        public ActionResult Create()
        {
            CarregarInstituicoes();
            return View(new BioterioViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BioterioViewModel bioterio)
        {
            NormalizarBioterioViewModel(bioterio);
            ValidarCamposNormalizados(bioterio, ModelState);

            if (!ModelState.IsValid)
            {
                CarregarInstituicoes(bioterio.IdInstituicao);
                return View(bioterio);
            }

            var instituicao = instituicaoService.Get(bioterio.IdInstituicao);
            if (instituicao == null)
            {
                ModelState.AddModelError(nameof(bioterio.IdInstituicao), "A instituição selecionada não existe.");
                CarregarInstituicoes(bioterio.IdInstituicao);
                return View(bioterio);
            }

            try
            {
                var bioterioDB = mapper.Map<Bioterio>(bioterio);
                bioterioService.Create(bioterioDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível salvar o biotério. {ex.InnerException?.Message ?? ex.Message}");
                CarregarInstituicoes(bioterio.IdInstituicao);
                return View(bioterio);
            }
        }

        public ActionResult Edit(uint id)
        {
            var bioterio = bioterioService.Get(id);
            if (bioterio == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<BioterioViewModel>(bioterio);
            CarregarInstituicoes(vm.IdInstituicao);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, BioterioViewModel bioterio)
        {
            if (id != bioterio.Id)
            {
                return BadRequest();
            }

            NormalizarBioterioViewModel(bioterio);
            ValidarCamposNormalizados(bioterio, ModelState);

            if (!ModelState.IsValid)
            {
                CarregarInstituicoes(bioterio.IdInstituicao);
                return View(bioterio);
            }

            var atual = bioterioService.Get(id);
            if (atual == null)
            {
                return NotFound();
            }

            var instituicao = instituicaoService.Get(bioterio.IdInstituicao);
            if (instituicao == null)
            {
                ModelState.AddModelError(nameof(bioterio.IdInstituicao), "A instituição selecionada não existe.");
                CarregarInstituicoes(bioterio.IdInstituicao);
                return View(bioterio);
            }

            try
            {
                var bioterioDB = mapper.Map<Bioterio>(bioterio);
                bioterioService.Update(bioterioDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível atualizar o biotério. {ex.InnerException?.Message ?? ex.Message}");
                CarregarInstituicoes(bioterio.IdInstituicao);
                return View(bioterio);
            }
        }

        public ActionResult Delete(uint id)
        {
            var bioterio = bioterioService.Get(id);
            if (bioterio == null)
            {
                return NotFound();
            }

            var vm = mapper.Map<BioterioViewModel>(bioterio);

            var instituicao = instituicaoService.Get(bioterio.IdInstituicao);
            vm.NomeInstituicao = instituicao?.Nome;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, BioterioViewModel bioterio)
        {
            try
            {
                bioterioService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = bioterioService.Get(id);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<BioterioViewModel>(existente);
                var instituicao = instituicaoService.Get(existente.IdInstituicao);
                vm.NomeInstituicao = instituicao?.Nome;

                ModelState.AddModelError(string.Empty, $"Não foi possível excluir o biotério. {ex.InnerException?.Message ?? ex.Message}");
                return View(vm);
            }
        }
    }
}
