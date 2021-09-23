using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly AppContext context;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(AppContext context, ILogger<WeatherForecastController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var user = new User { Name = "shekhar1" };
            context.Users.Add(user);

            var userHis = new UserHistory { CreatedAt = DateTime.Now, User = new User { Id = user.Id } };
            context.UserHistories.Add(userHis);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
