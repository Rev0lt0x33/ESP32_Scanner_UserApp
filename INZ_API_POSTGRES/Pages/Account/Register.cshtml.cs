using INZ_API_POSTGRES.Schemas;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;


namespace INZ_API_POSTGRES.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        [BindProperty]
        [Required(ErrorMessage = "Username is required.")]
        public string InputUsername { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string InputEmail { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string InputPassword { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Compare("InputPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public RegisterModel(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {


               

                var existingUser = await _userManager.FindByNameAsync(InputUsername);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Username is already taken.");
                    return Page();
                }

                existingUser = await _userManager.FindByEmailAsync(InputEmail);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email is already registered.");
                    return Page();
                }

                var user = new Users { UserName = InputUsername, Email = InputEmail};
                var result = await _userManager.CreateAsync(user, InputPassword);

                if (result.Succeeded)
                {

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("/App/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
