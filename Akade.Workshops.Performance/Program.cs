using Akade.Workshops.Performance.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IHistoricalWeatherDataRepository, HistoricalWeatherDataRepository>();

builder.Services.AddDbContext<HistoricalWeatherDataContext>(c => c.UseSqlite("Data Source=historicaldata.db; Cache=Private")
            .AddInterceptors(new SqLiteSimulateNetworkLatency()));

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    HistoricalWeatherDataContext context = scope.ServiceProvider.GetRequiredService<HistoricalWeatherDataContext>();
    // _ = await context.Database.EnsureDeletedAsync();
    _ = await context.Database.EnsureCreatedAsync();
}

using (IServiceScope scope = app.Services.CreateScope())
{
    IHistoricalWeatherDataRepository repo = scope.ServiceProvider.GetRequiredService<IHistoricalWeatherDataRepository>();
    // await SourceDataLoader.LoadAndSaveAsync(repo);
}


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

class SqLiteSimulateNetworkLatency : DbCommandInterceptor
{
    public override async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1);
        return new RemoteSimulatingDataReader(await base.ReaderExecutedAsync(command, eventData, result, cancellationToken));
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        Thread.Sleep(0);
        return new RemoteSimulatingDataReader(base.ReaderExecuted(command, eventData, result));
    }


}

sealed class RemoteSimulatingDataReader : DbDataReader
{
    private readonly DbDataReader _reader;

    private static int GetNextNetworkDelay()
    {
        return Random.Shared.NextSingle() > 0.98 ? 1 : 0;
    }

    public RemoteSimulatingDataReader(DbDataReader reader)
    {
        _reader = reader;
    }

    public override object this[int ordinal] => _reader[ordinal];

    public override object this[string name] => _reader[name];

    public override int Depth => _reader.Depth;

    public override int FieldCount => _reader.FieldCount;

    public override bool HasRows => _reader.HasRows;

    public override bool IsClosed => _reader.IsClosed;

    public override int RecordsAffected => _reader.RecordsAffected;

    public override bool GetBoolean(int ordinal)
    {
        return _reader.GetBoolean(ordinal);
    }

    public override byte GetByte(int ordinal)
    {
        return _reader.GetByte(ordinal);
    }

    public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
    {
        return _reader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
    }

    public override char GetChar(int ordinal)
    {
        return _reader.GetChar(ordinal);
    }

    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
    {
        return _reader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
    }

    public override string GetDataTypeName(int ordinal)
    {
        return _reader.GetDataTypeName(ordinal);
    }

    public override DateTime GetDateTime(int ordinal)
    {
        return _reader.GetDateTime(ordinal);
    }

    public override decimal GetDecimal(int ordinal)
    {
        return _reader.GetDecimal(ordinal);
    }

    public override double GetDouble(int ordinal)
    {
        return _reader.GetDouble(ordinal);
    }

    public override IEnumerator GetEnumerator()
    {
        return _reader.GetEnumerator();
    }

    public override Type GetFieldType(int ordinal)
    {
        return _reader.GetFieldType(ordinal);
    }

    public override float GetFloat(int ordinal)
    {
        return _reader.GetFloat(ordinal);
    }

    public override Guid GetGuid(int ordinal)
    {
        return _reader.GetGuid(ordinal);
    }

    public override short GetInt16(int ordinal)
    {
        return _reader.GetInt16(ordinal);
    }

    public override int GetInt32(int ordinal)
    {
        return _reader.GetInt32(ordinal);
    }

    public override long GetInt64(int ordinal)
    {
        return _reader.GetInt64(ordinal);
    }

    public override string GetName(int ordinal)
    {
        return _reader.GetName(ordinal);
    }

    public override int GetOrdinal(string name)
    {
        return _reader.GetOrdinal(name);
    }

    public override string GetString(int ordinal)
    {
        return _reader.GetString(ordinal);
    }

    public override object GetValue(int ordinal)
    {
        return _reader.GetValue(ordinal);
    }

    public override int GetValues(object[] values)
    {
        return _reader.GetValues(values);
    }

    public override bool IsDBNull(int ordinal)
    {
        return _reader.IsDBNull(ordinal);
    }

    public override bool NextResult()
    {
        Thread.Sleep(GetNextNetworkDelay());
        return _reader.NextResult();
    }

    public override async Task<bool> NextResultAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(GetNextNetworkDelay(), cancellationToken);
        return await base.NextResultAsync(cancellationToken);
    }

    public override bool Read()
    {
        Thread.Sleep(GetNextNetworkDelay());
        return _reader.Read();
    }

    public override void Close()
    {
        _reader.Close();
    }

    public override Task CloseAsync()
    {
        return _reader.CloseAsync();
    }

    public override ValueTask DisposeAsync()
    {
        return _reader.DisposeAsync();
    }

    protected override void Dispose(bool disposing)
    {
        _reader.Dispose();
    }

    public override bool Equals(object? obj)
    {
        return _reader.Equals(obj);
    }

    public override Task<ReadOnlyCollection<DbColumn>> GetColumnSchemaAsync(CancellationToken cancellationToken = default)
    {
        return _reader.GetColumnSchemaAsync(cancellationToken);
    }

    public override T GetFieldValue<T>(int ordinal)
    {
        return _reader.GetFieldValue<T>(ordinal);
    }

    public override Task<T> GetFieldValueAsync<T>(int ordinal, CancellationToken cancellationToken)
    {
        return _reader.GetFieldValueAsync<T>(ordinal, cancellationToken);
    }

    public override int GetHashCode()
    {
        return _reader.GetHashCode();
    }

    public override Type GetProviderSpecificFieldType(int ordinal)
    {
        return _reader.GetProviderSpecificFieldType(ordinal);
    }

    public override object GetProviderSpecificValue(int ordinal)
    {
        return _reader.GetProviderSpecificValue(ordinal);
    }

    public override int GetProviderSpecificValues(object[] values)
    {
        return _reader.GetProviderSpecificValues(values);
    }

    public override DataTable? GetSchemaTable()
    {
        return _reader.GetSchemaTable();
    }

    public override Task<DataTable?> GetSchemaTableAsync(CancellationToken cancellationToken = default)
    {
        return _reader.GetSchemaTableAsync(cancellationToken);
    }

    public override Stream GetStream(int ordinal)
    {
        return _reader.GetStream(ordinal);
    }

    public override TextReader GetTextReader(int ordinal)
    {
        return _reader.GetTextReader(ordinal);
    }

    public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken)
    {
        return _reader.IsDBNullAsync(ordinal, cancellationToken);
    }

    public override async Task<bool> ReadAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(GetNextNetworkDelay(), cancellationToken);
        return await _reader.ReadAsync(cancellationToken);
    }

    public override string ToString()
    {
        return _reader.ToString();
    }

    public override int VisibleFieldCount => _reader.VisibleFieldCount;
}
