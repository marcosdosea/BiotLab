using BiotLabWeb.Areas.Identity.Data;
using BiotLabWeb.Helpers;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            // MVC + Razor Pages
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Contexto principal do sistema
            builder.Services.AddDbContext<BiotlabContext>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("BiotLabConnection")));

            // Contexto do Identity
            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("IdentityConnection")));

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
            builder.Services.AddTransient<IEmailSender, EmailSender>();

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
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}