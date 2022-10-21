using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace OpenTelemetryForAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Tracer _tracer;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, TracerProvider provider)
        {
            _logger = logger;
            _tracer = provider.GetTracer(TelemetryConstants.MyAppSource);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var tags = new TagList();
            tags.Add("user-agent", Request.Headers.UserAgent);
            TelemetryConstants.HitsCounter.Add(1, tags);
            using var mySpan = _tracer.StartActiveSpan("MyOp").SetAttribute("httpTracer", HttpContext.TraceIdentifier);
            mySpan.AddEvent($"Received HTTP request from {Request.Headers.UserAgent}");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}