using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using INZ_API_POSTGRES.Schemas;
using Microsoft.AspNetCore.Identity;

namespace INZ_API_POSTGRES.Pages.Account
{
    public class LogoutModel : PageModel
    {

       
        private readonly ILogger<LogoutModel> _logger;
        private readonly SignInManager<Users> _signInManager;
        public LogoutModel(ILogger<LogoutModel> logger, SignInManager<Users> signInManager)
        {

            _logger = logger;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
			_logger.LogInformation("logut");
			if (_signInManager.IsSignedIn(User))
            {
                _signInManager.SignOutAsync();
				
			}

			return RedirectToPage("Login");
		}

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("logut");
            // Sign the user out
            await _signInManager.SignOutAsync();

            // Redirect to a page after logout (you can customize the redirect)
            return RedirectToPage("Login");
        }


    }
}
