using INZ_API_POSTGRES.Schemas;
using INZ_API_POSTGRES.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace INZ_API_POSTGRES.Pages.Account
{
    public class SettingsModel : PageModel
    {
        private readonly UserManager<Users> _userManager; 
        private readonly ILogger<SettingsModel> _logger;
		private readonly DatabaseContext _databaseContext;
		private Users _user;
        public string Username { get; set; }
        public string Email { get; set; }

		public string Apikey { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "New Password is required.")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[BindProperty]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }


        public SettingsModel(UserManager<Users> userManager,ILogger<SettingsModel> logger,DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _logger = logger;
			_databaseContext = databaseContext;
		}

		public async Task OnGet()
        {
			_logger.LogInformation("change password get");
			await GetUserData();

		}

		private async Task GetUserData()
        {
			Users loggedUser = await _userManager.GetUserAsync(User);
			if (loggedUser == null) return;
            Email = loggedUser.Email;
            Username = loggedUser.UserName;
			Apikey = loggedUser.ApiKey;
			

		}

		public async Task<IActionResult> OnPostChangePasswordAsync()
		{
			
			if(ModelState.IsValid)
			{
				Users loggedUser = await _userManager.GetUserAsync(User);
                var changedPassword = await _userManager.ChangePasswordAsync(loggedUser, Password, NewPassword);

				if (changedPassword.Succeeded)
				{
					_logger.LogInformation("changed password");
					TempData["SuccessMessagePassword"] = "Password changed successfully";
				}

				foreach(var error in changedPassword.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				

			}
			await GetUserData();
			return Page();
		}


		public async Task<IActionResult> OnPostGenerateNewKeyAsync()
		{
			ModelState.Clear();
			Users loggedUser = await _userManager.GetUserAsync(User);

			string newApiKey;
			bool isUnique;
			do
			{
				newApiKey = ApiKeyGeneration.GenerateApiKey();
				isUnique = !await ApiKeyExistsAsync(newApiKey);
			}
			while (!isUnique);

			loggedUser.ApiKey = newApiKey;
			_logger.LogInformation(newApiKey);

			var result = await _userManager.UpdateAsync(loggedUser);
			if (result.Succeeded)
			{
				TempData["SuccessMessageKey"] = "New API key generated successfully.";
			}
			else
			{
				TempData["SuccessMessageKey"] = "Error generating API Key.";
				foreach (var error in result.Errors)
				{
					_logger.LogError($"Error generating new API key: {error.Description}");
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			await GetUserData();
			return Page();
		}

		public async Task<bool> ApiKeyExistsAsync(string apiKey)
		{
			
			return await _databaseContext.Users.AnyAsync(u => u.ApiKey == apiKey);
		}


	}
}
