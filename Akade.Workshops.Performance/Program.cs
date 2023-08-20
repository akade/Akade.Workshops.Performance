using Akade.Workshops.Performance.Api.Data;
using Akade.Workshops.Performance.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

using ToCsvMeterListener listener = new();
listener.Start();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IHistoricalWeatherDataRepository, HistoricalWeatherDataRepository>();

builder.Services.AddDbContext<HistoricalWeatherDataContext>(
    c => c.UseSqlite("Data Source=historicaldata.db")
          // Removing this interceptor is cheating:
          // It adds some simulated network latency (very low compared to a cloud db but reasonable for the exercise) to the in-process SqLite DB
          // To challenge any performance improvements, you can increase the default values in the .ctor (lowering is cheating)
          .AddInterceptors(new NetworkLatencySimulatorInterceptor()));

WebApplication app = builder.Build();

// The weather data is updated daily (if we would use all data), this will reload everything
// If you want to focus on query performance first, comment out the deleted and load calls after you initially waited
await EnsureDBDeletedAsync(app);
await EnsureDBCreatedAsync(app);
await LoadAndSaveDataAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

async Task EnsureDBCreatedAsync(WebApplication app)
{
    using IServiceScope scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<HistoricalWeatherDataContext>().Database.EnsureCreatedAsync();
}

async Task EnsureDBDeletedAsync(WebApplication app)
{
    using IServiceScope scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<HistoricalWeatherDataContext>().Database.EnsureDeletedAsync();
}

async Task LoadAndSaveDataAsync(WebApplication app)
{
    using IServiceScope scope = app.Services.CreateScope();
    await SourceDataLoader.LoadAndSaveAsync(scope.ServiceProvider.GetRequiredService<IHistoricalWeatherDataRepository>());
}