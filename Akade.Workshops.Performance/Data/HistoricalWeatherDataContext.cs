using Microsoft.EntityFrameworkCore;

namespace Akade.Workshops.Performance.Api.Data;

public class HistoricalWeatherDataContext : DbContext
{
    public HistoricalWeatherDataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<HistoricalWeatherData> HistoricalWeatherData { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<HistoricalWeatherData>().HasKey(x => new { x.StationCode, x.Date, x.ElementType });
        base.OnModelCreating(modelBuilder);
    }
}
