using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.JSInterop;

using Microsoft.AspNetCore.Http;

using Syncfusion.Blazor;

using System.Net;
using System.Threading.Tasks;

using WelcomeSite.Data;
using Ganss.XSS;
using System;
using WelcomeSite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WelcomeSite
{
    public class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public Startup(IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSyncfusionBlazor();


            services.AddControllersWithViews(options =>
            {
            });

            services.AddRazorPages();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
              .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(options => { /* Authentication options */ })
                .AddGitHub(options =>
                {
                    options.CallbackPath = $"https://{Configuration["hostname"]}/callback-github";
                    options.ClientId = "91e2fc985b12e8319456";
                    options.ClientSecret = "b8e7509ea8f1114c6b3e895f11fc1c0de39ce169";
                });

            services.AddServerSideBlazor();

            services.AddScoped<BrowserService>();

            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

            services.AddHttpContextAccessor();

            services.AddTransient<IHtmlSanitizer, HtmlSanitizer>();

            services.AddSingleton<NavManager>();

            // Set requirements for security
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                // Default User settings.
                options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Identity/Account/Login"; // Set here login path.
                options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // set here access denied path.
                options.SlidingExpiration = true; // resets cookie expiration if more than half way through lifespan
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // cookie validation time
                options.Cookie.Name = "WelcomeSiteCookie";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }

    public class BrowserService
    {
        private readonly IJSRuntime _js;

        public BrowserService(IJSRuntime js)
        {
            _js = js;
        }

#pragma warning disable AsyncFixer01 // Unnecessary async/await usage
        public async Task<BrowserDimension> GetDimensions()
        {
            return await _js.InvokeAsync<BrowserDimension>("getDimensions");
        }
#pragma warning restore AsyncFixer01 // Unnecessary async/await usage

    }

    public class BrowserDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
