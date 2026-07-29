using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace BiotLabWeb.Controllers.Tests;

[TestClass]
public class AuthorizationRoleTests
{
    [TestMethod]
    public void InstituicaoController_PermiteConsultaParaAdministradorEPesquisadorSenior()
    {
        var roles = ObterRolesDoController(typeof(InstituicaoController));

        CollectionAssert.AreEquivalent(
            new[] { "Administrador", "PesquisadorSenior" },
            roles);
    }

    [TestMethod]
    public void InstituicaoController_RestringeAlteracoesAoAdministrador()
    {
        var nomesAcoesRestritas = new[] { "Create", "Edit", "Delete", "DeleteConfirmed" };
        var acoesRestritas = typeof(InstituicaoController)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Where(m => nomesAcoesRestritas.Contains(m.Name))
            .ToList();

        Assert.AreEqual(6, acoesRestritas.Count);

        foreach (var acao in acoesRestritas)
        {
            var roles = ObterRoles(acao.GetCustomAttribute<AuthorizeAttribute>()?.Roles);
            CollectionAssert.AreEquivalent(new[] { "Administrador" }, roles);
        }
    }

    [TestMethod]
    public void AdminUsuariosController_PermitePesquisadorSeniorEAdministrador()
    {
        var roles = ObterRolesDoController(typeof(AdminUsuariosController));

        CollectionAssert.AreEquivalent(
            new[] { "PesquisadorSenior", "Administrador" },
            roles);
    }

    [TestMethod]
    public void AdminUsuariosController_RestringeAcoesAdministrativasAoPesquisadorSenior()
    {
        var nomesAcoesRestritas = new[]
        {
            "NovoEstudante",
            "Bloquear",
            "Desbloquear",
            "Delete",
            "DeleteConfirmed"
        };
        var acoesRestritas = typeof(AdminUsuariosController)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Where(m => nomesAcoesRestritas.Contains(m.Name))
            .ToList();

        Assert.AreEqual(6, acoesRestritas.Count);

        foreach (var acao in acoesRestritas)
        {
            var roles = ObterRoles(acao.GetCustomAttribute<AuthorizeAttribute>()?.Roles);
            CollectionAssert.AreEquivalent(new[] { "PesquisadorSenior" }, roles);
        }
    }

    [TestMethod]
    public void Administrador_NaoTemAcessoAosDemaisModulos()
    {
        var nomesControladores = new[]
        {
            "AnestesicoController",
            "BioterioController",
            "EntradaanestesicoController",
            "EntradumController",
            "ExperimentoController",
            "FornecedorController",
            "GaiolaController",
            "GaiolaharemController",
            "HaremController",
            "ObituarioController",
            "PesquisadorController",
            "UsoanestesicoController"
        };
        var appAssembly = typeof(InstituicaoController).Assembly;

        foreach (var nomeController in nomesControladores)
        {
            var controller = appAssembly.GetType($"BiotLabWeb.Controllers.{nomeController}");
            Assert.IsNotNull(controller, $"Controller {nomeController} não encontrado.");

            var roles = ObterRolesDoController(controller);

            CollectionAssert.Contains(roles, "PesquisadorSenior");
            CollectionAssert.DoesNotContain(roles, "Administrador");
        }
    }

    private static string[] ObterRolesDoController(Type controller)
    {
        return ObterRoles(controller.GetCustomAttribute<AuthorizeAttribute>()?.Roles);
    }

    private static string[] ObterRoles(string? roles)
    {
        return (roles ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
