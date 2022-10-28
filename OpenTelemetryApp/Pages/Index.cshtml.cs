using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenTelemetryApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        static HttpClient client = new HttpClient();
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            IEnumerable<Weather>? weathers;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7290/WeatherForecast"))
                {
                    response.Headers.Add("Request-Id", Activity.Current?.TraceId.ToString() ?? String.Empty);
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    weathers = JsonConvert.DeserializeObject<List<Weather>>(apiResponse);
                }
            }



            //var tags = new TagList();
            //tags.Add("user-agent", Request.Headers.UserAgent);
            //HitsCounter.Add(1, tags);
            //using var mySpan = _tracer.StartActiveSpan("MyOp").SetAttribute("httpTracer", HttpContext.TraceIdentifier);
            //mySpan.AddEvent($"Received HTTP request from {Request.Headers.UserAgent}");
            

        }
    }
}