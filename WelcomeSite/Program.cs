using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Syncfusion.Licensing;

namespace WelcomeSite
{
    public static class Program
    {
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="args">Command Line Arguments</param>
        static void Main(string[] args)
        {
            // Create Web App Host
            var host = CreateHostBuilder(args).Build();

            // Set Service Provider for App
            Startup.ServiceProvider = host.Services;

            // Register SyncFusion
            SyncfusionLicenseProvider.RegisterLicense(Startup.ApplicationConfiguration["SFKey"]);

            // Start the Web App
            host.Run();
        }

        /// <summary>
        /// Build the Web App Host.
        /// </summary>
        /// <param name="args">Command Line Arguments</param>
        /// <returns><seealso cref="IHostBuilder"/></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
