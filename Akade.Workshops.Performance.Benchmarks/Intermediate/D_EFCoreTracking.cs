
/* Unmerged change from project 'Akade.Workshops.Performance.Benchmarks (net6.0)'
Before:
using BenchmarkDotNet.Attributes;
After:
using Akade;
using Akade.Workshops;
using Akade.Workshops.Performance;
using Akade.Workshops.Performance.Benchmarks;
using Akade.Workshops.Performance.Benchmarks;
using Akade.Workshops.Performance.Benchmarks.Updates;
using BenchmarkDotNet.Attributes;
*/
using Akade.Workshops.Performance.Benchmarks.Introductory;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Akade.Workshops.Performance.Benchmarks.Intermediate;

/// <summary>
/// This benchmark demonstrate a simple mistake that completly alters the behavior of your queries. As for now, it is very slow
/// 
/// Can you make it faster? And explain what happend?
/// - Note: it might seem not so slow, but remember, it is using an inmemory db without any network whatsoever.
/// - Hint:
/// - Hint:
/// </summary>
[FastJob]
public class D_EFCoreTracking
{
    private readonly SqliteConnection _connection;
    private readonly SimpleDbContext _context = null!;
    private readonly DataEntry[] _data = Enumerable.Range(1, 2000).Select(x => new DataEntry() { Id = x, Value = x * 2 }).ToArray();

    public D_EFCoreTracking()
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

    [Benchmark]
    public int Count_values_over_1k_AsNoTrackign()
    {
        _context.ChangeTracker.Clear();
        List<DataEntry> elements = _context.DataEntries.AsNoTracking().Where(x => x.Value > 1000).ToList();
        return elements.Sum(x => x.Value);
    }
}
