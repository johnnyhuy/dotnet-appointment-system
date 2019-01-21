using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Rmit.Asr.Application.Areas.Identity.Pages.Student
{
    [AllowAnonymous]
    public class StudentLogoutModel : PageModel
    {
        private readonly SignInManager<Data.Student> _signInManager;
        private readonly ILogger<StudentLogoutModel> _logger;

        public StudentLogoutModel(SignInManager<Data.Student> signInManager, ILogger<StudentLogoutModel> logger)
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