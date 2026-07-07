using Microsoft.AspNetCore.Identity;

namespace BiotLabWeb.Areas.Identity.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UsuarioIdentity>>();

            // 1. Criar roles
            string[] roles = { "Administrador", "Estudante" };

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
                    TipoUsuario = "Administrador"
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

                if (adminUser.TipoUsuario != "Administrador")
                {
                    adminUser.TipoUsuario = "Administrador";
                    precisaAtualizar = true;
                }

                if (string.IsNullOrWhiteSpace(adminUser.NomeCompleto))
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

            // 4. Garantir role Administrador
            var isAdmin = await userManager.IsInRoleAsync(adminUser, "Administrador");
            if (!isAdmin)
            {
                var addRoleResult = await userManager.AddToRoleAsync(adminUser, "Administrador");
                if (!addRoleResult.Succeeded)
                {
                    throw new Exception(
                        $"Erro ao vincular role Administrador ao admin inicial: {string.Join(" | ", addRoleResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}