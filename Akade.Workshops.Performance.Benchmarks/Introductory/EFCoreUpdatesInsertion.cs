using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Akade.Workshops.Performance.Benchmarks.Introductory;

/// <summary>
/// This benchmark demonstrate that you can test against different dependency versions and also reap some benefits by staying up to date.
/// If interested, take a look at how FastJob is using the NugetPackages-String to instruct BenchmarkDotNet.
/// 
/// Note that the requirement to clear the db requires an IterationCleanup. This needs to be done after every Invocation, hence for
/// our custom attribute, we need to specify an UnrollFactor of 1, so that BenchmarkDotNet will never combine multiple invocations.
/// (When using <see cref="SimpleJobAttribute"/>, this should be automatic)
/// </summary>
[FastJob(RuntimeMoniker.Net70, unrollFactor: 1, maxIterationCount: 20, nugetPackages: "Microsoft.EntityFrameworkCore.Sqlite, 6.0.20", baseline: true)]
[FastJob(RuntimeMoniker.Net70, unrollFactor: 1, maxIterationCount: 20, nugetPackages: "Microsoft.EntityFrameworkCore.Sqlite, 7.0.9")]
public class EFCoreUpdatesInsertion
{
    private readonly SqliteConnection _connection;
    private readonly SimpleDbContext _context = null!;
    private readonly DataEntry[] _data = Enumerable.Range(1, 2000).Select(x => new DataEntry() { Id = x, Value = x * 2 }).ToArray();


    public EFCoreUpdatesInsertion()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new SimpleDbContext(new DbContextOptionsBuilder().UseSqlite(_connection).EnableSensitiveDataLogging().Options);
        _ = _context.Database.EnsureCreated();
    }

    [Benchmark]
    public void Insertion()
    {
        _context.AddRange(_data);
        _ = _context.SaveChanges();
    }


    [IterationCleanup]
    public void Clean()
    {
        _context.ChangeTracker.Clear();
        if (_context.Database.ExecuteSqlRaw($"DELETE FROM {nameof(SimpleDbContext.DataEntries)};") != _data.Length)
        {
            throw new InvalidOperationException();
        }
    }
}

public class SimpleDbContext : DbContext
{
    public SimpleDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<DataEntry> DataEntries { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<DataEntry>().ToTable(nameof(DataEntries)).HasKey(x => x.Id);
        _ = modelBuilder.Entity<DataEntry>().Property(x => x.Id).ValueGeneratedNever();
        base.OnModelCreating(modelBuilder);
    }
}

public class DataEntry
{
    public int Id { get; set; }

    public int Value { get; set; }
}
