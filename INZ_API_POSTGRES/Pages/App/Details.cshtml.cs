
using INZ_API_POSTGRES.Schemas;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace INZ_API_POSTGRES.Pages.App
{
    public class DataModel : PageModel
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<DataModel> _logger;
		private readonly UserManager<Users> _userManager;
		public string DeviceName { get; set; }
		public IList<Data> DataList { get; set; }

		public int DevID { get; set; }

		public DataModel(DatabaseContext databaseContext, ILogger<DataModel> logger, UserManager<Users> userManager)
		{
			_databaseContext = databaseContext;
			_logger = logger;
			_userManager = userManager;
			DataList = new List<Data>();
			DeviceName = "";
			DevID = 0;
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			DevID = id;
			_logger.LogCritical($"{DevID}");
			await GetData(DevID);
			return Page();
		}



		private async Task GetData(int deviceId)
		{
			var loggedUser = await _userManager.GetUserAsync(User);
			if (loggedUser == null) return;

			_logger.LogInformation(loggedUser.Id);

			DeviceName = _databaseContext.Device.Where(device => device.Id == deviceId).Select(d => d.Name).FirstOrDefault();
			DataList = await _databaseContext.Datas
				.Where(data => data.DeviceId == deviceId && data.Device.UserId == loggedUser.Id).ToListAsync();
			DevID = deviceId;
        }

		public async Task<IActionResult> OnPostRemoveInfoAsync(int id,int devID)
		{
			_logger.LogCritical($"{DevID}");
			_logger.LogInformation("test");
			var deviceToRemove = await _databaseContext.Datas.FindAsync(id);

			if (deviceToRemove != null)
			{
				_databaseContext.Datas.Remove(deviceToRemove);
				await _databaseContext.SaveChangesAsync();

			}
			
			await GetData(devID);
			return Page();
		}



	}
}
