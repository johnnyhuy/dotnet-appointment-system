using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;

[assembly: HostingStartup(typeof(Rmit.Asr.Application.Areas.Identity.IdentityHostingStartup))]
namespace Rmit.Asr.Application.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ApplicationDataContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DefaultConnection")));
                
                services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                    {
                        options.Password.RequiredLength = 3;
                        options.Password.RequireDigit = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                    })
                    .AddEntityFrameworkStores<ApplicationDataContext>()
                    .AddDefaultTokenProviders();
                
                services.AddIdentityCore<Student>()
                    .AddRoles<IdentityRole>()
                    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Student, IdentityRole>>()
                    .AddEntityFrameworkStores<ApplicationDataContext>()
                    .AddDefaultTokenProviders();
                
                services.AddIdentityCore<Staff>()
                    .AddRoles<IdentityRole>()
                    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Staff, IdentityRole>>()
                    .AddEntityFrameworkStores<ApplicationDataContext>()
                    .AddDefaultTokenProviders();
                
                services.AddScoped<SignInManager<ApplicationUser>>();
                services.AddScoped<UserManager<ApplicationUser>>();
                services.AddScoped<SignInManager<Student>>();
                services.AddScoped<UserManager<Student>>();
                services.AddScoped<SignInManager<Staff>>();
                services.AddScoped<UserManager<Staff>>();
            });
        }
    }
}