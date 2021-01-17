using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistroCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace aspnetcore5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IDistro distro;

        public WeatherForecastController(IDistro distro)
        {
            this.distro = distro;

            Random rng = new Random();

            var wfArray = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = $"wf{index}",
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = $"weather data {index}"
            });

            this.distro.AddRange("wf", wfArray);
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            distro.Remove<WeatherForecast>("wf", "wf3");

            distro.Remove<WeatherForecast>("wf", new WeatherForecast { Id = "wf4" });

            return distro.List<WeatherForecast>("wf").ToArray();
        }
    }
}
