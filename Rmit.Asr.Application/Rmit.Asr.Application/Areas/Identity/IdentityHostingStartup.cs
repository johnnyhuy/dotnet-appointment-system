using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
                services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                    {
                        options.Password.RequiredLength = 3;
                        options.Password.RequireDigit = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                    })
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddDefaultTokenProviders()
                    .AddDefaultUI()
                    .AddEntityFrameworkStores<ApplicationDataContext>();
                
                services.AddIdentityCore<Student>()
                    .AddRoles<IdentityRole>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddEntityFrameworkStores<ApplicationDataContext>();
                
                services.AddIdentityCore<Staff>()
                    .AddRoles<IdentityRole>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddEntityFrameworkStores<ApplicationDataContext>();

                services.AddScoped<SignInManager<ApplicationUser>>();
                services.AddScoped<UserManager<ApplicationUser>>();
                services.AddScoped<SignInManager<Staff>>();
                services.AddScoped<UserManager<Staff>>();
                services.AddScoped<SignInManager<Student>>();
                services.AddScoped<UserManager<Student>>();
            });
        }
    }
}