using AutoMapper;
using BiotLabWeb.Models;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;


namespace BiotLabWeb.Controllers
{
    [Authorize(Roles = "Administrador,PesquisadorSenior")]
    public class InstituicaoController : Controller
    {
    
        private readonly IInstituicaoService instituicaoService;
        private readonly IMapper mapper;

        public InstituicaoController(IInstituicaoService instituicaoService, IMapper mapper)
        {
            this.instituicaoService = instituicaoService;
            this.mapper = mapper;
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

        private static void NormalizarInstituicaoViewModel(InstituicaoViewModel instituicao)
        {
            instituicao.Nome = instituicao.Nome?.Trim() ?? string.Empty;
            instituicao.Cnpj = SomenteDigitos(instituicao.Cnpj);
            instituicao.Cep = SomenteDigitos(instituicao.Cep);
            instituicao.Rua = LimparTextoOpcional(instituicao.Rua);
            instituicao.Bairro = LimparTextoOpcional(instituicao.Bairro);
            instituicao.Cidade = LimparTextoOpcional(instituicao.Cidade);
            instituicao.Numero = LimparTextoOpcional(instituicao.Numero);
            instituicao.Complemento = LimparTextoOpcional(instituicao.Complemento);
            instituicao.Estado = (instituicao.Estado ?? string.Empty).Trim().ToUpper();
            instituicao.Telefone1 = SomenteDigitos(instituicao.Telefone1);
            instituicao.Telefone2 = string.IsNullOrWhiteSpace(instituicao.Telefone2)
                ? null
                : SomenteDigitos(instituicao.Telefone2);
            instituicao.Email = instituicao.Email?.Trim() ?? string.Empty;
        }

        private static void ValidarCamposNormalizados(InstituicaoViewModel instituicao, ModelStateDictionary modelState)
        {
            if (instituicao.Cep.Length != 8)
                modelState.AddModelError(nameof(instituicao.Cep), "O CEP deve conter 8 dígitos.");

            if (instituicao.Cnpj.Length != 14)
                modelState.AddModelError(nameof(instituicao.Cnpj), "O CNPJ deve conter 14 dígitos.");

            if (instituicao.Estado.Length != 2)
                modelState.AddModelError(nameof(instituicao.Estado), "O estado deve conter 2 caracteres.");
        }

        public ActionResult Index()
        {
            var instituicoes = instituicaoService.GetAll().ToList();
            var vm = mapper.Map<List<InstituicaoViewModel>>(instituicoes);
            return View(vm);
        }

        public ActionResult Details(uint id)
        {
            var instituicao = instituicaoService.Get(id);
            if (instituicao == null)
                return NotFound();

            var vm = mapper.Map<InstituicaoViewModel>(instituicao);
            return View(vm);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View(new InstituicaoViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstituicaoViewModel instituicao)
        {
            NormalizarInstituicaoViewModel(instituicao);
            ValidarCamposNormalizados(instituicao, ModelState);

            if (!ModelState.IsValid)
                return View(instituicao);

            try
            {
                var instituicaoDB = mapper.Map<Instituicao>(instituicao);
                instituicaoService.Create(instituicaoDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível salvar a instituição. {ex.InnerException?.Message ?? ex.Message}");
                return View(instituicao);
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(uint id)
        {
            var instituicao = instituicaoService.Get(id);
            if (instituicao == null)
                return NotFound();

            var vm = mapper.Map<InstituicaoViewModel>(instituicao);
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, InstituicaoViewModel instituicao)
        {
            if (id != instituicao.Id)
                return BadRequest();

            NormalizarInstituicaoViewModel(instituicao);
            ValidarCamposNormalizados(instituicao, ModelState);

            if (!ModelState.IsValid)
                return View(instituicao);

            var atual = instituicaoService.Get(id);
            if (atual == null)
                return NotFound();

            try
            {
                var instituicaoDB = mapper.Map<Instituicao>(instituicao);
                instituicaoService.Update(instituicaoDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível atualizar a instituição. {ex.InnerException?.Message ?? ex.Message}");
                return View(instituicao);
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(uint id)
        {
            var instituicao = instituicaoService.Get(id);
            if (instituicao == null)
                return NotFound();

            var vm = mapper.Map<InstituicaoViewModel>(instituicao);
            return View(vm);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(uint id)
        {
            try
            {
                instituicaoService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = instituicaoService.Get(id);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<InstituicaoViewModel>(existente);
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, $"Não foi possível excluir a instituição. {ex.InnerException?.Message ?? ex.Message}");
                return View(vm);
            }
        }
    }
}
