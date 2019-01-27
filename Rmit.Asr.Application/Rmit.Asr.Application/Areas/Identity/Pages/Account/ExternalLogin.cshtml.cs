using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Rmit.Asr.Application.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ExternalLoginModel> _logger;
        private ApplicationDataContext _context;

        public ExternalLoginModel(
            ApplicationDataContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ExternalLoginModel> logger)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel : ApplicationUser
        {
            [Required]
            public string Role { get; set; }
            
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            string redirectUrl = Url.Page("./ExternalLogin", "Callback", new { returnUrl });
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }
            
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            
            // Get the external login email and check if it already exists
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                Input = new InputModel
                {
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                };
                
                if (await _context.Staff.AnyAsync(u => u.Email == Input.Email))
                {
                    ErrorMessage = "User email has already been registered by a users.";
                    
                    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }
            }
            
            // Sign in the user with this external login provider if the user already has a login.
            SignInResult result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                
                return LocalRedirect(returnUrl);
            }
            
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            
            ReturnUrl = returnUrl;
            LoginProvider = info.LoginProvider;

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            
            // Get the information about the user from the external login provider
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (_context.Staff.Any(u => u.Email == Input.Email))
            {
                ErrorMessage = "User email has already been registered as a staff.";
                
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            
            if (ModelState.IsValid)
            {
                ApplicationUser user;

                if (string.Equals(Input.Role, Models.Student.RoleName, StringComparison.CurrentCultureIgnoreCase))
                    user = new Models.Student
                    {
                        FirstName = Input.FirstName, 
                        LastName = Input.LastName,
                        UserName = Input.Email,
                        Email = Input.Email,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };
                else if (string.Equals(Input.Role, Models.Staff.RoleName, StringComparison.CurrentCultureIgnoreCase))
                    user = new Models.Staff
                    {
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        UserName = Input.Email,
                        Email = Input.Email,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };
                else
                {
                    ErrorMessage = "Student or staff user has not been selected.";
                
                    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }

                IdentityResult result = await _userManager.CreateAsync(user);
                
                if (result.Succeeded)
                {
                    if (string.Equals(Input.Role, Models.Student.RoleName, StringComparison.CurrentCultureIgnoreCase))
                        await _userManager.AddToRoleAsync(user, Models.Student.RoleName);
                    else if (string.Equals(Input.Role, Models.Staff.RoleName, StringComparison.CurrentCultureIgnoreCase))
                        await _userManager.AddToRoleAsync(user, Models.Staff.RoleName);
                    
                    result = await _userManager.AddLoginAsync(user, info);
                    
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        
                        return LocalRedirect(returnUrl);
                    }
                }
                
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            LoginProvider = info.LoginProvider;
            ReturnUrl = returnUrl;
            
            return Page();
        }
    }
}
