using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rmit.Asr.Application.Areas.Identity.Data;
using Rmit.Asr.Application.Areas.Identity.Models;

[assembly: HostingStartup(typeof(Rmit.Asr.Application.Areas.Identity.IdentityHostingStartup))]
namespace Rmit.Asr.Application.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<IdentityDataContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DefaultConnection")));
                
                services.AddDefaultIdentity<ApplicationUser>()
                    .AddEntityFrameworkStores<IdentityDataContext>()
                    .AddDefaultTokenProviders();
                
                services.AddScoped<SignInManager<ApplicationUser>>();
                services.AddScoped<SignInManager<Student>>();
                services.AddScoped<SignInManager<Staff>>();
                
                services.AddScoped<UserManager<ApplicationUser>>();
                services.AddScoped<UserManager<Student>>();
                services.AddScoped<UserManager<Staff>>();
            });
        }
    }
}