using BiotLabWeb.Areas.Identity.Data;
using BiotLabWeb.Helpers;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Service;

namespace BiotLabWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var applyingSchemaPatches = args.Contains("--apply-schema-patches");
            var runningEndToEndTests =
                string.Equals(
                    Environment.GetEnvironmentVariable("BIOTLAB_E2E_MODE"),
                    "1",
                    StringComparison.Ordinal);

            if (applyingSchemaPatches || runningEndToEndTests)
            {
                builder.Logging.ClearProviders();
                builder.Logging.AddConsole();
            }

            if (runningEndToEndTests)
            {
                builder.Services.AddDataProtection()
                    .UseEphemeralDataProtectionProvider();
            }

            // MVC + Razor Pages
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // AutoMapper
            builder.Services.AddAutoMapper(_ => { }, AppDomain.CurrentDomain.GetAssemblies());

            var biotLabConnection = builder.Configuration.GetConnectionString("BiotLabConnection")
                ?? throw new InvalidOperationException("Connection string 'BiotLabConnection' não configurada.");
            var identityConnection = builder.Configuration.GetConnectionString("IdentityConnection")
                ?? throw new InvalidOperationException("Connection string 'IdentityConnection' não configurada.");

            // Contexto principal do sistema
            builder.Services.AddDbContext<BiotlabContext>(options =>
                options.UseMySQL(biotLabConnection));

            // Contexto do Identity
            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseMySQL(identityConnection));

            // Serviços da aplicação
            builder.Services.AddTransient<IGaiolaService, GaiolaService>();
            builder.Services.AddTransient<IInstituicaoService, InstituicaoService>();
            builder.Services.AddTransient<IHaremService, HaremService>();
            builder.Services.AddTransient<IExperimentoService, ExperimentoService>();
            builder.Services.AddTransient<IObituarioService, ObituarioService>();
            builder.Services.AddTransient<IBioterioService, BioterioService>();
            builder.Services.AddTransient<IFornecedorService, FornecedorService>();
            builder.Services.AddTransient<IGaiolaharemService, GaiolaharemService>();
            builder.Services.AddTransient<IUsoanestesicoService, UsoanestesicoService>();
            builder.Services.AddTransient<IPesquisadorService, PesquisadorService>();
            builder.Services.AddTransient<IEntradaanestesicoService, EntradaanestesicoService>();
            builder.Services.AddTransient<IAnestesicosService, AnestesicoService>();
            builder.Services.AddTransient<IEntradumService, EntradumService>();
            // Serviço de envio de e-mail
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            // Identity
            builder.Services
                .AddIdentity<UsuarioIdentity, IdentityRole>(options =>
                {
                    // SignIn
                    options.SignIn.RequireConfirmedAccount = true;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    // Senha
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Usuário
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                    // Lockout
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;
                })
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            // Garante que mudanças de role sejam aplicadas já na próxima requisição.
            builder.Services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            // Cookie de autenticação
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "BiotLab.Auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;

                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

            var app = builder.Build();

            if (applyingSchemaPatches)
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<BiotlabContext>();
                var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
                await ApplySchemaPatchesAsync(context);
                await ApplyIdentitySchemaPatchesAsync(identityContext);
                return;
            }

            // Seed de roles + admin inicial
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();

                await IdentitySeed.SeedAsync(services, configuration);
            }

            // Pipeline HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            // Encerra imediatamente a sessão de qualquer usuário bloqueado.
            app.Use(async (context, next) =>
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var userManager = context.RequestServices
                        .GetRequiredService<UserManager<UsuarioIdentity>>();
                    var usuario = await userManager.GetUserAsync(context.User);

                    if (usuario != null && await userManager.IsLockedOutAsync(usuario))
                    {
                        var signInManager = context.RequestServices
                            .GetRequiredService<SignInManager<UsuarioIdentity>>();

                        await signInManager.SignOutAsync();
                        context.Response.Redirect("/Identity/Account/Lockout");
                        return;
                    }
                }

                await next();
            });

            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }

        private static async Task ApplySchemaPatchesAsync(BiotlabContext context)
        {
            var tituloExists = await context.Database
                .SqlQueryRaw<int>("""
                    SELECT COUNT(*) AS Value
                    FROM information_schema.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'experimento'
                      AND COLUMN_NAME = 'titulo'
                    """)
                .SingleAsync();

            if (tituloExists == 0)
            {
                await context.Database.ExecuteSqlRawAsync("""
                    ALTER TABLE experimento
                    ADD COLUMN titulo VARCHAR(100) NOT NULL DEFAULT 'Projeto sem titulo' AFTER id;
                    """);

                Console.WriteLine("Schema patch aplicado: experimento.titulo criado.");
            }
            else
            {
                Console.WriteLine("Schema patch ignorado: experimento.titulo ja existe.");
            }

            var columnInfo = await context.Database
                .SqlQueryRaw<string>("""
                    SELECT CONCAT(DATABASE(), '.', TABLE_NAME, '.', COLUMN_NAME, ' ', COLUMN_TYPE, ' ', IS_NULLABLE) AS Value
                    FROM information_schema.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'experimento'
                    AND COLUMN_NAME = 'titulo'
                    """)
                .ToListAsync();

            foreach (var info in columnInfo)
            {
                Console.WriteLine(info);
            }

            var tituloProjetoLegacyExists = await context.Database
                .SqlQueryRaw<int>("""
                    SELECT COUNT(*) AS Value
                    FROM information_schema.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'experimento'
                      AND COLUMN_NAME = 'tituloProjeto'
                    """)
                .SingleAsync();

            if (tituloProjetoLegacyExists > 0)
            {
                await context.Database.ExecuteSqlRawAsync("""
                    UPDATE experimento
                    SET titulo = CASE
                        WHEN titulo IS NULL OR titulo = '' OR titulo = 'Projeto sem titulo'
                            THEN COALESCE(NULLIF(tituloProjeto, ''), CONCAT('Experimento ', id))
                        ELSE titulo
                    END;
                    """);
            }
            else
            {
                await context.Database.ExecuteSqlRawAsync("""
                    UPDATE experimento
                    SET titulo = COALESCE(NULLIF(titulo, ''), CONCAT('Experimento ', id));
                    """);
            }

            await context.Database.ExecuteSqlRawAsync("""
                ALTER TABLE experimento
                MODIFY COLUMN titulo VARCHAR(100) NOT NULL,
                MODIFY COLUMN cepa VARCHAR(50) NULL;
                """);

            var experimentoPesquisadorExists = await context.Database
                .SqlQueryRaw<int>("""
                    SELECT COUNT(*) AS Value
                    FROM information_schema.TABLES
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'experimentoPesquisador'
                    """)
                .SingleAsync();

            if (experimentoPesquisadorExists == 0)
            {
                await context.Database.ExecuteSqlRawAsync("""
                    CREATE TABLE experimentoPesquisador (
                        idExperimento INT UNSIGNED NOT NULL,
                        idPesquisador INT UNSIGNED NOT NULL,
                        PRIMARY KEY (idExperimento, idPesquisador),
                        INDEX fk_ExperimentoPesquisador_Pesquisador1_idx (idPesquisador),
                        CONSTRAINT fk_ExperimentoPesquisador_Experimento1
                            FOREIGN KEY (idExperimento)
                            REFERENCES experimento (id)
                            ON DELETE CASCADE
                            ON UPDATE NO ACTION,
                        CONSTRAINT fk_ExperimentoPesquisador_Pesquisador1
                            FOREIGN KEY (idPesquisador)
                            REFERENCES pesquisador (id)
                            ON DELETE RESTRICT
                            ON UPDATE NO ACTION
                    );
                    """);
            }

            await context.Database.ExecuteSqlRawAsync("""
                INSERT IGNORE INTO experimentoPesquisador (idExperimento, idPesquisador)
                SELECT id, idPesquisador
                FROM experimento
                WHERE idPesquisador IS NOT NULL;
                """);

            var origemPaiExists = await context.Database
                .SqlQueryRaw<int>("""
                    SELECT COUNT(*) AS Value
                    FROM information_schema.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'harem'
                      AND COLUMN_NAME = 'origemPai'
                    """)
                .SingleAsync();

            if (origemPaiExists == 0)
            {
                await context.Database.ExecuteSqlRawAsync("""
                    ALTER TABLE harem
                    ADD COLUMN origemPai VARCHAR(100) NOT NULL DEFAULT 'Nao informado' AFTER dataNascimento;
                    """);
            }

            var origemMaeExists = await context.Database
                .SqlQueryRaw<int>("""
                    SELECT COUNT(*) AS Value
                    FROM information_schema.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'harem'
                      AND COLUMN_NAME = 'origemMae'
                    """)
                .SingleAsync();

            if (origemMaeExists == 0)
            {
                await context.Database.ExecuteSqlRawAsync("""
                    ALTER TABLE harem
                    ADD COLUMN origemMae VARCHAR(100) NOT NULL DEFAULT 'Nao informado' AFTER origemPai;
                    """);
            }

            Console.WriteLine("Schema patch completo aplicado/verificado.");
        }

        private static async Task ApplyIdentitySchemaPatchesAsync(IdentityContext context)
        {
            var convitesUsuariosExists = await context.Database
                .SqlQueryRaw<int>("""
                    SELECT COUNT(*) AS Value
                    FROM information_schema.TABLES
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'ConvitesUsuarios'
                    """)
                .SingleAsync();

            if (convitesUsuariosExists == 0)
            {
                await context.Database.ExecuteSqlRawAsync("""
                    CREATE TABLE ConvitesUsuarios (
                        Id INT UNSIGNED NOT NULL AUTO_INCREMENT,
                        NomeCompleto VARCHAR(150) NOT NULL,
                        Email VARCHAR(256) NOT NULL,
                        Perfil VARCHAR(30) NOT NULL,
                        Codigo VARCHAR(100) NOT NULL,
                        CriadoEm DATETIME(6) NOT NULL,
                        AceitoEm DATETIME(6) NULL,
                        UsuarioId VARCHAR(450) NULL,
                        PRIMARY KEY (Id),
                        UNIQUE INDEX IX_ConvitesUsuarios_Codigo (Codigo),
                        INDEX IX_ConvitesUsuarios_Email_AceitoEm (Email, AceitoEm)
                    );
                    """);

                Console.WriteLine("Schema patch aplicado: ConvitesUsuarios criado.");
            }
            else
            {
                Console.WriteLine("Schema patch ignorado: ConvitesUsuarios ja existe.");
            }
        }
    }
}
