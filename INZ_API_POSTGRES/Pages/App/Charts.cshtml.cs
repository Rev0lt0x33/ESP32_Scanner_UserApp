using INZ_API_POSTGRES.Schemas;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace INZ_API_POSTGRES.Pages.App
{
    public class ChartsModel : PageModel
    {

		private readonly DatabaseContext _databaseContext;
		private readonly ILogger<ChartsModel> _logger;
		private readonly UserManager<Users> _userManager;

        public string LabelsJson { get; private set; }
        public string DataJson { get; private set; }
        [BindProperty]
        public DateTime InputDate { get; set; }



        public ChartsModel(DatabaseContext databaseContext, ILogger<ChartsModel> logger, UserManager<Users> userManager)
		{
			_databaseContext = databaseContext;
			_logger = logger;
			_userManager = userManager;
            

        }


		 
        public async Task OnGet()
        {
			
        }


        private async Task getChartData()
        {
			var loggedUser = await _userManager.GetUserAsync(User);
			if (loggedUser == null) return;

            var _dataList = await _databaseContext.Datas
				.Where(data => data.Device.UserId == loggedUser.Id && 
                data.Date.Day == InputDate.Date.Day && 
                data.Date.Month == InputDate.Date.Month).ToListAsync();      
            var groupedData = _dataList.GroupBy(d => new { Hour = d.Date.Hour })
                     .Select(g => new
                     {
                         Hour = g.Key.Hour,
                         Count = g.Count()
                     })
                     .OrderBy(g => g.Hour)
                     .ToList();

            var labels = groupedData.Select(g => $"{g.Hour}:00-{g.Hour + 1}:00").ToList();
            var data = groupedData.Select(g => g.Count).ToList();

            LabelsJson = Newtonsoft.Json.JsonConvert.SerializeObject(labels);
            DataJson = Newtonsoft.Json.JsonConvert.SerializeObject(data);

        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation(InputDate.Date.Date.ToString());
            await getChartData();
            
            _logger.LogInformation("tetsetestestestsetes");
            return Page();
        }

    }
}
