using Akade.Workshops.Performance.Benchmarks.Introductory;
using BenchmarkDotNet.Attributes;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Akade.Workshops.Performance.Benchmarks.Required;

/// <summary>
/// This benchmark demonstrate a simple mistake that completely alters the behavior of your queries.
/// Can you make it faster? And explain what happens?
/// Hints:
/// - Note: it might seem not so slow, but remember, it is using an in-memory db without any network whatsoever.
/// - SG93IGRvZXMgRUYgQ29yZSB0cmFuc2xhdGUgeW91ciBDIyBjb2RlIGludG8gU1FMPw==
/// - RUYgQ29yZSB1c2VzIEV4cHJlc3Npb24gVHJlZXMgZm9yIHRoYXQuIElRdWVyeWFibGUgaXMgdGhlIHN1cHBvcnRpbmcgdHlwZSBmb3IgdGhhdC4=
/// - QyMgYmluZHMgbWV0aG9kcyBiYXNlZCBvbiB0aGUgc3RhdGljIHR5cGU=
/// - VGhlIHJlcG9zaXRvcnkgcmV0dXJucyBJRW51bWVyYWJsZQ==
/// - QXMgSUVudW1lcmFibGUgaXMgdGhlIHN0YXRpYyB0eXBlIGZvciB0aGUgcXVlcnkgYW5kIG5vdCBJUXVlcnlhYmxlLCBFRiBoYXMgdG8gbWF0ZXJpYWxpemUgYWxsIGVudGl0aWVzIHRvIGV4ZWN1dGUgdGhlIHF1ZXJ5LCB3aGljaCBpcyBub3QgaW4gRXhwcmVzc2lvbiBUcmVlcyBidXQgY29tcGlsZWQgY29kZS4=
/// </summary>
[FastJob]
public class EFCoreQueries
{
    private readonly SqliteConnection _connection;
    private readonly SimpleDbContext _context = null!;
    private readonly DataEntry[] _data = Enumerable.Range(1, 2000).Select(x => new DataEntry() { Id = x, Value = x * 2 }).ToArray();
    private readonly SimpleDataRepository _repo;

    public EFCoreQueries()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new SimpleDbContext(new DbContextOptionsBuilder().UseSqlite(_connection).EnableSensitiveDataLogging().Options);
        _ = _context.Database.EnsureCreated();
        _context.AddRange(_data);
        _ = _context.SaveChanges();
        _context.ChangeTracker.Clear();
        _repo = new SimpleDataRepository(_context);
    }

    [Benchmark]
    public int Count_values_over_1k()
    {
        return _repo.Query().Where(x => x.Value > 1000).Count();
    }

}

public class SimpleDataRepository
{
    private readonly SimpleDbContext _context;

    public SimpleDataRepository(SimpleDbContext context)
    {
        _context = context;
    }

    public IEnumerable<DataEntry> Query()
    {
        return _context.DataEntries;
    }
}