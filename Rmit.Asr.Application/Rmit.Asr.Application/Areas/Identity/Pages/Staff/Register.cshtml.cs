using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.ViewModels;

namespace Rmit.Asr.Application.Areas.Identity.Pages.Staff
{
    [AllowAnonymous]
    public class StaffRegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<StaffRegisterModel> _logger;

        public StaffRegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<StaffRegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel : RegisterStaff
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            
            if (!ModelState.IsValid) return Page();

            string email = $"{Input.StaffId}@{Models.Staff.EmailSuffix}";
            var user = new Models.Staff
            {
                StaffId = Input.StaffId,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                UserName = email,
                Email = email
            };
            
            ApplicationUser findUser = await _userManager.FindByIdAsync(user.StaffId);

            if (findUser != null)
            {
                ModelState.AddModelError(string.Empty, $"User {user.StaffId} already exists.");
                
                return Page();
            }

            IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
                
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                    
                await _userManager.AddToRoleAsync(user, Models.Staff.RoleName);
                    
                await _signInManager.SignInAsync(user, false);

                TempData["StatusMessage"] = $"Successfully registered staff user {user.FirstName}, {user.StaffId}";
                TempData["AlertType"] = "success";

                return LocalRedirect(returnUrl);
            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
