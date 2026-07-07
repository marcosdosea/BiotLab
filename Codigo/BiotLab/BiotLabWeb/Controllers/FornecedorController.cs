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
    [Authorize(Roles = "Administrador")]
    public class FornecedorController : Controller
    {
        private readonly IFornecedorService fornecedorService;
        private readonly IInstituicaoService instituicaoService;
        private readonly IMapper mapper;

        public FornecedorController(IFornecedorService fornecedorService, IInstituicaoService instituicaoService, IMapper mapper)
        {
            this.fornecedorService = fornecedorService;
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

        private static void NormalizarFornecedor(FornecedorViewModel fornecedor)
        {
            fornecedor.Nome = fornecedor.Nome?.Trim() ?? string.Empty;
            fornecedor.Cnpj = SomenteDigitos(fornecedor.Cnpj);
            fornecedor.Cep = SomenteDigitos(fornecedor.Cep);
            fornecedor.Rua = LimparTextoOpcional(fornecedor.Rua);
            fornecedor.Bairro = LimparTextoOpcional(fornecedor.Bairro);
            fornecedor.Cidade = LimparTextoOpcional(fornecedor.Cidade);
            fornecedor.Numero = LimparTextoOpcional(fornecedor.Numero);
            fornecedor.Complemento = LimparTextoOpcional(fornecedor.Complemento);
            fornecedor.Estado = (fornecedor.Estado ?? string.Empty).Trim().ToUpper();
            fornecedor.Telefone1 = SomenteDigitos(fornecedor.Telefone1);
            fornecedor.Telefone2 = string.IsNullOrWhiteSpace(fornecedor.Telefone2) ? null : SomenteDigitos(fornecedor.Telefone2);
            fornecedor.Email = fornecedor.Email?.Trim() ?? string.Empty;
        }

        private static void ValidarCamposNormalizados(FornecedorViewModel fornecedor, ModelStateDictionary modelState)
        {
            if (fornecedor.Cep.Length != 8)
                modelState.AddModelError(nameof(fornecedor.Cep), "O CEP deve conter 8 dígitos.");

            if (fornecedor.Cnpj.Length != 14)
                modelState.AddModelError(nameof(fornecedor.Cnpj), "O CNPJ deve conter 14 dígitos.");

            if (fornecedor.Estado.Length != 2)
                modelState.AddModelError(nameof(fornecedor.Estado), "O estado deve conter 2 caracteres.");
        }

        public ActionResult Index()
        {
            var fornecedores = fornecedorService.GetAll();
            var vm = mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);
            return View(vm);
        }

        public ActionResult Details(uint id)
        {
            var fornecedor = fornecedorService.Get(id);
            if (fornecedor == null)
                return NotFound();

            var vm = mapper.Map<FornecedorViewModel>(fornecedor);
            return View(vm);
        }

        public ActionResult Create()
        {
            CarregarInstituicoes();
            return View(new FornecedorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FornecedorViewModel fornecedor)
        {
            NormalizarFornecedor(fornecedor);
            ValidarCamposNormalizados(fornecedor, ModelState);

            if (!ModelState.IsValid)
            {
                CarregarInstituicoes(fornecedor.IdInstituicao);
                return View(fornecedor);
            }

            if (instituicaoService.Get(fornecedor.IdInstituicao) == null)
            {
                ModelState.AddModelError(nameof(fornecedor.IdInstituicao), "A instituição selecionada não existe.");
                CarregarInstituicoes(fornecedor.IdInstituicao);
                return View(fornecedor);
            }

            try
            {
                var fornecedorDB = mapper.Map<Fornecedor>(fornecedor);
                fornecedorService.Create(fornecedorDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível salvar o fornecedor. {ex.InnerException?.Message ?? ex.Message}");
                CarregarInstituicoes(fornecedor.IdInstituicao);
                return View(fornecedor);
            }
        }

        public ActionResult Edit(uint id)
        {
            var fornecedor = fornecedorService.Get(id);
            if (fornecedor == null)
                return NotFound();

            var vm = mapper.Map<FornecedorViewModel>(fornecedor);
            CarregarInstituicoes(vm.IdInstituicao);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, FornecedorViewModel fornecedor)
        {
            if (id != fornecedor.Id)
                return BadRequest();

            NormalizarFornecedor(fornecedor);
            ValidarCamposNormalizados(fornecedor, ModelState);

            if (!ModelState.IsValid)
            {
                CarregarInstituicoes(fornecedor.IdInstituicao);
                return View(fornecedor);
            }

            var atual = fornecedorService.Get(id);
            if (atual == null)
                return NotFound();

            if (instituicaoService.Get(fornecedor.IdInstituicao) == null)
            {
                ModelState.AddModelError(nameof(fornecedor.IdInstituicao), "A instituição selecionada não existe.");
                CarregarInstituicoes(fornecedor.IdInstituicao);
                return View(fornecedor);
            }

            try
            {
                var fornecedorDB = mapper.Map<Fornecedor>(fornecedor);
                fornecedorService.Update(fornecedorDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Não foi possível atualizar o fornecedor. {ex.InnerException?.Message ?? ex.Message}");
                CarregarInstituicoes(fornecedor.IdInstituicao);
                return View(fornecedor);
            }
        }

        public ActionResult Delete(uint id)
        {
            var fornecedor = fornecedorService.Get(id);
            if (fornecedor == null)
                return NotFound();

            var vm = mapper.Map<FornecedorViewModel>(fornecedor);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, FornecedorViewModel fornecedor)
        {
            try
            {
                fornecedorService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var existente = fornecedorService.Get(id);
                if (existente == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<FornecedorViewModel>(existente);
                ModelState.AddModelError(string.Empty, $"Não foi possível excluir o fornecedor. {ex.InnerException?.Message ?? ex.Message}");
                return View(vm);
            }
        }
    }
}