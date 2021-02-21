using System;
using System.Net;

using Azure.Identity;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.JSInterop;

using Ganss.XSS;
using Syncfusion.Blazor;

using WelcomeSite.Data;
using WelcomeSite.Services;

namespace WelcomeSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            IdentityModelEventSource.ShowPII = Convert.ToBoolean(Configuration["ShowPii"]);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Exposes the ServiceProvider for the Host to the
        /// rest of the application.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Exposes the application configuration to the rest
        /// of the application.
        /// </summary>
        public static IConfiguration ApplicationConfiguration => ServiceProvider.GetService<IConfiguration>();

        /// <summary>
        /// Host Builder configuration.
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Adds the services to Dependency Injection.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable SyncFusion Blazor Integration.
            services.AddSyncfusionBlazor();

            // Provide controllers for MVC.
            services.AddControllersWithViews();

            // Provide Razor Pages for Identity.
            services.AddRazorPages();

            // Add Identity Services
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
              .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add Authentication and Authorization services.
            services.AddAuthentication()
                .AddGitHub(options =>
                {
                    options.CallbackPath = $"/callback-github";
                    options.ClientId = Configuration["GitHub_ClientId"];
                    options.ClientSecret = Configuration["GitHub_ClientSecret"];
                });

            // Add Blazor Server-Side Services
            services.AddServerSideBlazor();

            // JSInterop Helpers for getting Browser Info.
            services.AddScoped<BrowserService>();

            // DbContext Registration.
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

            // Allow accessing the HttpContext from components.
            services.AddHttpContextAccessor();

            // Register the HtmlSanitizer service.
            services.AddTransient<IHtmlSanitizer, HtmlSanitizer>();

            // Navigation management for swapping components.
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

            // Configure Cookies for Security.
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

            // More cookie options.
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = CookieSecurePolicy.SameAsRequest;
                options.Secure = CookieSecurePolicy.Always;
            });

            // This section registers the cryptographic services provided by
            // Azure.
            var blobStorageUri = Configuration["BlobStorage"];
            var keyVaultId = Configuration["KeyVaultUri"];
            var tenantId = Configuration["tenantId"];
            var clientId = Configuration["clientId"];
            var keyVaultSecret = Configuration["KeyVaultSecret"];
            var keyVaultSecretCredential = new ClientSecretCredential(tenantId, clientId, keyVaultSecret);
            var resolver = new Azure.Security.KeyVault.Keys.Cryptography.KeyResolver(keyVaultSecretCredential);
            var cloudBlobContainer = new CloudBlobContainer(new Uri(blobStorageUri));

            var dataProtection = services.AddDataProtection();

            dataProtection
                .PersistKeysToAzureBlobStorage(cloudBlobContainer,"Keys")
                .ProtectKeysWithAzureKeyVault(keyVaultId, resolver);

            // The GitHub OAuth provider creates the callback url as HTTP,
            // so when it is redirected to HTTPS Chrome 80 and above breaks
            // because the cookies are cross-site (despite having the same hostname).
            // This code fixes that error.
            services.ConfigureNonBreakingSameSiteCookies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Add this before any other middleware that might write cookies
            app.UseCookiePolicy();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Force HTTPS
            app.UseHttpsRedirection();

            // Serve static files, like our Markdown files.
            app.UseStaticFiles();

            // Enable Routing
            app.UseRouting();

            // Enable Authentication and Authorization Services.
            app.UseAuthentication();
            app.UseAuthorization();

            // Configure Routing
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
