
using Akade.Workshops.Performance.Benchmarks.Introductory;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Akade.Workshops.Performance.Benchmarks.Intermediate;

/// <summary>
/// This benchmark demonstrate a simple yet sometimes powerful change that you can often use if you do not want to modify any entity. 
/// Notes: 
/// - It might seem not so slow, but remember, it is using an in memory db without any network whatsoever.
/// - Do not count on the db side. That would defeat the purpose of this benchmark.
/// - Hint: RUYgQ29yZSB0cmFja3MgYWxsIGVudGl0aWVzIGJ5IGRlZmF1bHQuIFlvdSBjYW4gZGlzYWJsZSB0aGF0IHVzaW5nIEFzTm9UcmFja2luZygpIHdpdGhpbiB5b3VyIHF1ZXJ5Lg==
/// </summary>
[FastJob]
public class EFCoreTracking
{
    private readonly SqliteConnection _connection;
    private readonly SimpleDbContext _context = null!;
    private readonly DataEntry[] _data = Enumerable.Range(1, 2000).Select(x => new DataEntry() { Id = x, Value = x * 2 }).ToArray();

    public EFCoreTracking()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new SimpleDbContext(new DbContextOptionsBuilder().UseSqlite(_connection).EnableSensitiveDataLogging().Options);
        _ = _context.Database.EnsureCreated();
        _context.AddRange(_data);
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
    }

    [Benchmark]
    public int Count_values_over_1k()
    {
        _context.ChangeTracker.Clear();
        List<DataEntry> elements = _context.DataEntries.Where(x => x.Value > 1000).ToList();
        return elements.Sum(x => x.Value);
    }
}
