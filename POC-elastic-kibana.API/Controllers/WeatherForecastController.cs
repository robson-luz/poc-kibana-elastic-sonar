using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace POC_elastic_kibana.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly Serilog.ILogger _logger;

        public WeatherForecastController(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            //_logger.LogWarning("atenção...");
            Thread.Sleep(12000);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("/error")]
        public IEnumerable<WeatherForecast> GetError()
        {
            _logger.Error("LogError {campoTeste}", 50);

            throw new Exception("Exception teste");
        }
    }
}