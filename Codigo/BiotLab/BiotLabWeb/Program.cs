using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;
using BiotLabWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BiotLabWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IGaiolaService, GaiolaService>();
            builder.Services.AddTransient<IInstituicaoService, InstituicaoService>();
            builder.Services.AddTransient<IHaremService, HaremService>();
            builder.Services.AddTransient<IExperimentoService, ExperimentoService>();
            builder.Services.AddTransient<IObituarioService, ObituarioService>();
            builder.Services.AddTransient<IBioterioService, BioterioService>();
            builder.Services.AddTransient<IFornecedorService, FornecedorService>();
            builder.Services.AddTransient<IGaiolaharemService, GaiolaharemService>();
            builder.Services.AddTransient<IUsoanestesicoService, UsoanestesicoService>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddDbContext<BiotlabContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("BiotLabConnection")));
            builder.Services.AddDbContext<IdentityContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("IdentityConnection")));

            builder.Services.AddDefaultIdentity<UsuarioIdentity>(options =>
            {
                // SignIn settings
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;

                // Default User settings.
                options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

                // Default Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.Name = "BiotLabWebCookies";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
                options.Cookie.SameSite = SameSiteMode.Lax; 
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login"; 
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });




            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

            app.Run();
        }
    }
}
