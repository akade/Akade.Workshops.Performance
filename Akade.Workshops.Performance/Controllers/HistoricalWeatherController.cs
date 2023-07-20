using Akade.Workshops.Performance.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace Akade.Workshops.Performance.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HistoricalWeatherController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    private readonly ILogger<HistoricalWeatherController> _logger;
    private readonly IHistoricalWeatherDataRepository _repo;

    public HistoricalWeatherController(ILogger<HistoricalWeatherController> logger, IHistoricalWeatherDataRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    /// <summary>
    /// Queries the database for a distinct list of stations
    /// </summary>
    [HttpGet("stations")]
    public IEnumerable<Station> GetStations()
    {
        return _repo.Query()
                    .Select(x => new Station(x.StationCode, x.Station))
                    .Distinct()
                    .ToArray();
    }

    [HttpGet("stations/{stationCode}/{date}")]
    public ActionResult<HistoricalData> GetHistoricalData([FromRoute] string stationCode, [FromRoute] DateTime date)
    {
        DateOnly dateOnly = DateOnly.FromDateTime(date);
        IEnumerable<HistoricalWeatherData> data = _repo.Query()
                                                       .Where(x => x.StationCode == stationCode)
                                                       .Where(x => x.Date == dateOnly);

        if (!data.Any())
        {
            return NotFound();
        }

        Station station = new(data.First().StationCode, data.First().Station);

        return Ok(new HistoricalData(
            station,
            dateOnly,
            Precipitation: data.FirstOrDefault(data => data.ElementType == ElementType.PRCP)?.DataValue,
            Snow: data.FirstOrDefault(data => data.ElementType == ElementType.SNOW)?.DataValue,
            TMax: data.FirstOrDefault(data => data.ElementType == ElementType.TMAX)?.DataValue,
            TMin: data.FirstOrDefault(data => data.ElementType == ElementType.TMIN)?.DataValue,
            TAvg: data.FirstOrDefault(data => data.ElementType == ElementType.TAVG)?.DataValue));
    }



    //[HttpGet(Name = "GetWeatherForecast")]
    //public IEnumerable<WeatherForecast> Get()
    //{
    //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //    {
    //        Date = DateTime.Now.AddDays(index),
    //        TemperatureC = Random.Shared.Next(-20, 55),
    //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //    })
    //    .ToArray();
    //}
}

public record Station(string Code, string Name);

public record HistoricalData(Station Station, DateOnly Date, double? Precipitation, double? Snow, double? TMax, double? TMin, double? TAvg);