using Microsoft.AspNetCore.Mvc;

namespace Monq.Core.Redis.WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    static readonly string[] _summaries =
    [
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    readonly WeatherCacheService _weatherCacheService;

    public WeatherForecastController(WeatherCacheService weatherCacheService)
    {
        _weatherCacheService = weatherCacheService;
    }

    [HttpGet("GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var description = _weatherCacheService.GetWeatherDescription();

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = _summaries[Random.Shared.Next(_summaries.Length)]
        })
        .ToArray();
    }
}
