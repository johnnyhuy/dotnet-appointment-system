using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Rmit.Asr.Application.Areas.Identity.Pages.Staff
{
    [AllowAnonymous]
    public class StaffLogoutModel : PageModel
    {
        private readonly SignInManager<Data.Staff> _signInManager;
        private readonly ILogger<StaffLogoutModel> _logger;

        public StaffLogoutModel(SignInManager<Data.Staff> signInManager, ILogger<StaffLogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }
    }
}