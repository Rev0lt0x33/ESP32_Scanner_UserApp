using INZ_API_POSTGRES.Schemas;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace INZ_API_POSTGRES.Pages.App
{
    public class DevicesModel : PageModel
    {
        private readonly DatabaseContext _databaseContext;
        private readonly UserManager<Users> _userManager;
		private readonly ILogger<DevicesModel> _logger;
		public IList<Devices> UserDevices { get; set; }

        public DevicesModel(DatabaseContext context, UserManager<Users> userManager, ILogger<DevicesModel> logger) {
        
            _databaseContext = context;
            _userManager = userManager;
            _logger = logger;
            UserDevices = new List<Devices>();
            
        }
        
        public async Task<IActionResult> OnGetAsync()
        {

            await GetDevices();
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveDeviceAsync(int id)
        {
			_logger.LogInformation("test");
			var deviceToRemove = await _databaseContext.Device.FindAsync(id);
            
			if (deviceToRemove != null)
			{		
				_databaseContext.Device.Remove(deviceToRemove);
				await _databaseContext.SaveChangesAsync();
				
			}

			await GetDevices();
			return Page();
		}


        private async Task GetDevices()
        {
			var loggedUser = await _userManager.GetUserAsync(User);

			if (loggedUser == null) return; 

			UserDevices = await _databaseContext.Device
				.Where(device => device.UserId == loggedUser.Id)
				.ToListAsync();

		}

        public IActionResult OnPostNavDetails(int id)
        {
            return RedirectToPage("Details", new {id = id});
        }


	}   
}
