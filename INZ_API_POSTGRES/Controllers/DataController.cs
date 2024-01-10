using INZ_API_POSTGRES.Schemas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Security.Permissions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace INZ_API_POSTGRES.Controllers
{
    [Route("data/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        public class postModel
        {
            public string Name { get; set; }
            public string DeviceAddress { get; set; }
            public string ApiKey { get; set; }
            public string Mac { get; set; }
        }



        private readonly DatabaseContext _dbContext;
        private readonly ILogger<DataController> _logger;
        public DataController(DatabaseContext dbcontext, ILogger<DataController> logger)
        {
            _dbContext = dbcontext;
            _logger = logger;   
        }

        // POST api/<DataController>
        [HttpPost]
        public IActionResult Post([FromBody] postModel model)

        {
            _logger.LogCritical(model.ApiKey.ToString());
            Users ?user= _dbContext.Users
                   .FirstOrDefault(user => user.ApiKey == model.ApiKey);
            if (user == null)
            {
                return BadRequest("API Key not found.");
            }
            Devices ?UserDevice = _dbContext.Device
                .FirstOrDefault(device => device.ApiKey == user.Id && device.Mac ==model.DeviceAddress);
            if(UserDevice == null)
            {
                UserDevice = new Devices()
                {
                    Name = model.Name,
                    Mac = model.DeviceAddress,
                    UserId = user.Id,
                    Users = user,
                    ApiKey = model.ApiKey,
                };
                _dbContext.Add(UserDevice);
                _dbContext.SaveChanges();
                _logger.LogInformation("New Device Created");
            }
			UserDevice.Name=model.Name;
			Data newData = new Data() 
            {
                Device = UserDevice,
                Mac = model.Mac,
                DeviceId = UserDevice.Id,
                Date = DateTime.UtcNow
            };
			
			_dbContext.Add(newData);
            _dbContext.SaveChanges();
            return Ok();


        }
    }
}
