namespace Akade.Workshops.Performance.Api.Data;
public interface IHistoricalWeatherDataRepository
{
    public Task SaveWeatherDataAsync(HistoricalWeatherData data);

    public IEnumerable<HistoricalWeatherData> Query();
}

public class HistoricalWeatherDataRepository : IHistoricalWeatherDataRepository
{
    private readonly HistoricalWeatherDataContext _dataContext;

    public HistoricalWeatherDataRepository(HistoricalWeatherDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IEnumerable<HistoricalWeatherData> Query()
    {
        return _dataContext.HistoricalWeatherData;
    }

    public async Task SaveWeatherDataAsync(HistoricalWeatherData data)
    {
        _ = _dataContext.Add(data);
        _ = await _dataContext.SaveChangesAsync();
    }
}
