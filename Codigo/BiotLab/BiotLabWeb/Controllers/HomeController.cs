using BiotLabWeb.Areas.Identity.Data;
using BiotLabWeb.Models;
using BiotLabWeb.Models.Home;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BiotLabWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IInstituicaoService _instituicaoService;
        private readonly IBioterioService _bioterioService;
        private readonly IPesquisadorService _pesquisadorService;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEntradumService _entradumService;
        private readonly IEntradaanestesicoService _entradaAnestesicoService;
        private readonly IUsoanestesicoService _usoAnestesicoService;
        private readonly IExperimentoService _experimentoService;
        private readonly IGaiolaService _gaiolaService;
        private readonly IHaremService _haremService;
        private readonly IGaiolaharemService _gaiolaharemService;
        private readonly IAnestesicosService _anestesicoService;
        private readonly IObituarioService _obituarioService;
        private readonly UserManager<UsuarioIdentity> _userManager;

        public HomeController(
            IInstituicaoService instituicaoService,
            IBioterioService bioterioService,
            IPesquisadorService pesquisadorService,
            IFornecedorService fornecedorService,
            IEntradumService entradumService,
            IEntradaanestesicoService entradaAnestesicoService,
            IUsoanestesicoService usoAnestesicoService,
            IExperimentoService experimentoService,
            IGaiolaService gaiolaService,
            IHaremService haremService,
            IGaiolaharemService gaiolaharemService,
            IAnestesicosService anestesicoService,
            IObituarioService obituarioService,
            UserManager<UsuarioIdentity> userManager)
        {
            _instituicaoService = instituicaoService;
            _bioterioService = bioterioService;
            _pesquisadorService = pesquisadorService;
            _fornecedorService = fornecedorService;
            _entradumService = entradumService;
            _entradaAnestesicoService = entradaAnestesicoService;
            _usoAnestesicoService = usoAnestesicoService;
            _experimentoService = experimentoService;
            _gaiolaService = gaiolaService;
            _haremService = haremService;
            _gaiolaharemService = gaiolaharemService;
            _anestesicoService = anestesicoService;
            _obituarioService = obituarioService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var vm = new HomeDashboardViewModel
            {
                TotalInstituicoes = _instituicaoService.GetAll().Count(),
                TotalBioterios = _bioterioService.GetAll().Count(),
                TotalPesquisadores = _pesquisadorService.GetAll().Count(),
                TotalFornecedores = _fornecedorService.GetAll().Count(),
                TotalEntradas = _entradumService.GetAll().Count(),
                TotalEntradaAnestesico = _entradaAnestesicoService.GetAll().Count(),
                TotalUsoAnestesico = _usoAnestesicoService.GetAll().Count(),
                TotalUsuarios = _userManager.Users.Count(),

                TotalExperimentos = _experimentoService.GetAll().Count(),
                TotalGaiolas = _gaiolaService.GetAll().Count(),
                TotalHarems = _haremService.GetAll().Count(),
                TotalGaiolaHarems = _gaiolaharemService.GetAll().Count(),
                TotalAnestesicos = _anestesicoService.GetAll().Count(),
                TotalObituarios = _obituarioService.GetAll().Count()
            };

            return View(vm);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}