using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace WelcomeSite
{
    public static class Program
    {
        //private static readonly SurveyQuestion[] surveyQuestions = new[]
        //{
        //    new SurveyQuestion
        //    {
        //        QuestionText = "What is your name?",
        //        QuestionIsPrivate = true
        //    },
        //    new SurveyQuestion
        //    {
        //        QuestionText = "What is your role on the team?"
        //    },
        //    new SurveyQuestion
        //    {
        //        QuestionText = "What is something I should know about you?",
        //        QuestionIsPrivate = true
        //    },
        //    new SurveyQuestion
        //    {
        //        QuestionText = "How would you improve this survey website?"
        //    }
        //};

        static void Main(string[] args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzk5NzAyQDMxMzgyZTM0MmUzMElrTWYvUERRNlBVLzE3M3FXZ1ZvZzR5RW9lRU5MYUJOUTVUZkI1QXROUU09");

            var host = CreateHostBuilder(args).Build();

            Startup.ServiceProvider = host.Services;

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
