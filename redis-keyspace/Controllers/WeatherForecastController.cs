using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace redis_keyspace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ITaskServices _svc;

        public WeatherForecastController(ITaskServices svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            await _svc.DoTaskAsync();
            System.Console.WriteLine("done here");
            return "done";

        }
    }
}
