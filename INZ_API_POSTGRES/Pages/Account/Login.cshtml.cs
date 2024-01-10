using INZ_API_POSTGRES.Schemas;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace INZ_API_POSTGRES.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; }

        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        public LoginModel(ILogger<LoginModel> logger, SignInManager<Users> signInManager)
        {

            _signInManager = signInManager;
            _logger = logger;
        }


        public IActionResult OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("/App/Devices");
            }
            return Page();


        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {          
                var result = await _signInManager.PasswordSignInAsync(Username, Password, RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("redir home");
                    return RedirectToPage("../App/Devices");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            return Page();


        }
    }
}
