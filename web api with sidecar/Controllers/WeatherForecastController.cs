using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace web_api_with_sidecar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Version = 2
            })
            .ToArray();

            // Send a log to the sidecar
            var logData = new { message = "Weather forecast data retrieved" };
            var jsonContent = new StringContent(JsonSerializer.Serialize(logData), Encoding.UTF8, "application/json");

            try
            {
                // Send the log to the sidecar container
                var response = await _httpClient.PostAsync("http://localhost:5001/log", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Log sent to sidecar successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to send log to sidecar. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending log to sidecar: {ex.Message}");
            }

            //Send a log to the sidecar1
            try
            {
                // Send the log to the sidecar container
                var response = await _httpClient.PostAsync("http://localhost:5002/log", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Log sent to sidecar1 successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to send log to sidecar1. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending log to sidecar1: {ex.Message}");
            }

            return Ok(forecasts);
        }
    }

}