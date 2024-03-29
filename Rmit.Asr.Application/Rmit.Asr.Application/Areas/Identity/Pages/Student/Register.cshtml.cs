using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.ViewModels;

namespace Rmit.Asr.Application.Areas.Identity.Pages.Student
{
    [AllowAnonymous]
    public class StudentRegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<StudentRegisterModel> _logger;

        public StudentRegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<StudentRegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel : RegisterStudent
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
            
            string email = $"{Input.StudentId}@{Models.Student.EmailSuffix}";
            var user = new Models.Student
            {
                StudentId = Input.StudentId,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                UserName = email,
                Email = email
            };

            ApplicationUser findUser = await _userManager.FindByIdAsync(user.StudentId);

            if (findUser != null)
            {
                ModelState.AddModelError(string.Empty, "User already exists.");
            }
            
            IdentityResult result = await _userManager.CreateAsync(user, Input.Password);

            if (ModelState.IsValid && result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                await _userManager.AddToRoleAsync(user, Models.Student.RoleName);
                
                await _signInManager.SignInAsync(user, false);

                TempData["StatusMessage"] = $"Successfully registered student user {user.FirstName}, {user.StudentId}";
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
