using Microsoft.AspNetCore.Identity;

using System.Security.Claims;

namespace BiotLabWeb.Areas.Identity.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UsuarioIdentity>>();

            // 1. Criar roles
            string[] roles = { "PesquisadorSenior", "Administrador", "Estudante" };

            foreach (var role in roles)
            {
                var exists = await roleManager.RoleExistsAsync(role);
                if (!exists)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            $"Erro ao criar role '{role}': {string.Join(" | ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }

            // Separar, uma única vez, a antiga role Administrador (que representava
            // PesquisadorSenior) da nova role Administrador com acesso limitado.
            const string migrationClaimType = "BiotLabMigration";
            const string migrationClaimValue = "SepararAdministradorEPesquisadorSeniorV1";
            var pesquisadorSeniorRole = await roleManager.FindByNameAsync("PesquisadorSenior")
                ?? throw new Exception("Role PesquisadorSenior nao encontrada.");
            var migrationClaims = await roleManager.GetClaimsAsync(pesquisadorSeniorRole);

            if (!migrationClaims.Any(c =>
                    c.Type == migrationClaimType &&
                    c.Value == migrationClaimValue))
            {
                var antigosAdministradores = await userManager.GetUsersInRoleAsync("Administrador");

                foreach (var usuario in antigosAdministradores)
                {
                    if (!await userManager.IsInRoleAsync(usuario, "PesquisadorSenior"))
                    {
                        var addResult = await userManager.AddToRoleAsync(usuario, "PesquisadorSenior");
                        if (!addResult.Succeeded)
                        {
                            throw new Exception(
                                $"Erro ao migrar usuario para PesquisadorSenior: {string.Join(" | ", addResult.Errors.Select(e => e.Description))}");
                        }
                    }

                    var removeResult = await userManager.RemoveFromRoleAsync(usuario, "Administrador");
                    if (!removeResult.Succeeded)
                    {
                        throw new Exception(
                            $"Erro ao remover role Administrador antiga: {string.Join(" | ", removeResult.Errors.Select(e => e.Description))}");
                    }

                    usuario.TipoUsuario = "PesquisadorSenior";
                    var updateResult = await userManager.UpdateAsync(usuario);
                    if (!updateResult.Succeeded)
                    {
                        throw new Exception(
                            $"Erro ao atualizar perfil migrado: {string.Join(" | ", updateResult.Errors.Select(e => e.Description))}");
                    }

                    var stampResult = await userManager.UpdateSecurityStampAsync(usuario);
                    if (!stampResult.Succeeded)
                    {
                        throw new Exception(
                            $"Erro ao invalidar sessao do perfil migrado: {string.Join(" | ", stampResult.Errors.Select(e => e.Description))}");
                    }
                }

                var claimResult = await roleManager.AddClaimAsync(
                    pesquisadorSeniorRole,
                    new Claim(migrationClaimType, migrationClaimValue));

                if (!claimResult.Succeeded)
                {
                    throw new Exception(
                        $"Erro ao registrar migracao de roles: {string.Join(" | ", claimResult.Errors.Select(e => e.Description))}");
                }
            }

            // 2. Ler admin inicial do appsettings.json
            var adminEmail = configuration["IdentitySeed:AdminEmail"];
            var adminPassword = configuration["IdentitySeed:AdminPassword"];
            var adminNome = configuration["IdentitySeed:AdminNome"];

            if (string.IsNullOrWhiteSpace(adminEmail) ||
                string.IsNullOrWhiteSpace(adminPassword) ||
                string.IsNullOrWhiteSpace(adminNome))
            {
                throw new Exception("Configuração IdentitySeed incompleta no appsettings.json.");
            }

            // 3. Criar admin inicial se não existir
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new UsuarioIdentity
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    NomeCompleto = adminNome,
                    TipoUsuario = "PesquisadorSenior"
                };

                var createResult = await userManager.CreateAsync(adminUser, adminPassword);

                if (!createResult.Succeeded)
                {
                    throw new Exception(
                        $"Erro ao criar usuário administrador inicial: {string.Join(" | ", createResult.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                bool precisaAtualizar = false;

                if (!adminUser.EmailConfirmed)
                {
                    adminUser.EmailConfirmed = true;
                    precisaAtualizar = true;
                }

                if (adminUser.TipoUsuario != "PesquisadorSenior")
                {
                    adminUser.TipoUsuario = "PesquisadorSenior";
                    precisaAtualizar = true;
                }

                if (string.IsNullOrWhiteSpace(adminUser.NomeCompleto) ||
                    adminUser.NomeCompleto == "PesquisadorSenior" ||
                    adminUser.NomeCompleto == "Administrador BiotLab")
                {
                    adminUser.NomeCompleto = adminNome;
                    precisaAtualizar = true;
                }

                if (precisaAtualizar)
                {
                    var updateResult = await userManager.UpdateAsync(adminUser);
                    if (!updateResult.Succeeded)
                    {
                        throw new Exception(
                            $"Erro ao atualizar administrador inicial: {string.Join(" | ", updateResult.Errors.Select(e => e.Description))}");
                    }
                }
            }

            // 4. Garantir role PesquisadorSenior no usuário inicial
            var isPesquisadorSenior = await userManager.IsInRoleAsync(adminUser, "PesquisadorSenior");
            if (!isPesquisadorSenior)
            {
                var addRoleResult = await userManager.AddToRoleAsync(adminUser, "PesquisadorSenior");
                if (!addRoleResult.Succeeded)
                {
                    throw new Exception(
                        $"Erro ao vincular role PesquisadorSenior ao usuario inicial: {string.Join(" | ", addRoleResult.Errors.Select(e => e.Description))}");
                }
            }

            if (await userManager.IsInRoleAsync(adminUser, "Administrador"))
            {
                var removeRoleResult = await userManager.RemoveFromRoleAsync(adminUser, "Administrador");
                if (!removeRoleResult.Succeeded)
                {
                    throw new Exception(
                        $"Erro ao remover role Administrador do usuario inicial: {string.Join(" | ", removeRoleResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
