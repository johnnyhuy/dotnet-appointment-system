using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Rmit.Asr.Application.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Rmit.Asr.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred seeding the DB.");
                    Console.WriteLine(ex);
                }
            }

            BuildWebHost(args).Run();
            //CreateWebHostBuilder(args).Build().Run();
        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            //WebHost.CreateDefaultBuilder(args)
                //.UseStartup<Startup>();

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();


    }
}
