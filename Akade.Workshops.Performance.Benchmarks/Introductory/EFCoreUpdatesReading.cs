using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Akade.Workshops.Performance.Benchmarks.Introductory;

/// <summary>
/// This benchmark demonstrate that you can test against different dependency versions and also reap some benefits by staying up to date.
/// If interested, take a look at how FastJob is using the NugetPackages-String to instruct BenchmarkDotNet.
/// </summary>
[FastJob(RuntimeMoniker.Net70, nugetPackages: "Microsoft.EntityFrameworkCore.Sqlite, 6.0.20", baseline: true)]
[FastJob(RuntimeMoniker.Net70, nugetPackages: "Microsoft.EntityFrameworkCore.Sqlite, 7.0.9")]
public class EFCoreUpdatesReading
{
    private readonly SqliteConnection _connection;
    private readonly SimpleDbContext _context = null!;
    private readonly DataEntry[] _data = Enumerable.Range(1, 2000).Select(x => new DataEntry() { Id = x, Value = x * 2 }).ToArray();


    public EFCoreUpdatesReading()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new SimpleDbContext(new DbContextOptionsBuilder().UseSqlite(_connection).EnableSensitiveDataLogging().Options);
        _ = _context.Database.EnsureCreated();
    }

    [Benchmark]
    public int Reading()
    {
        return _context.DataEntries.Where(d => d.Value > 100 & d.Value < 150).ToArray().Length; // Not counting on the db on purpose

    }

    [GlobalSetup]
    public void InitWitData()
    {
        _context.AddRange(_data);
        _ = _context.SaveChanges();
    }
}
