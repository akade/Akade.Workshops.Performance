using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Akade.Workshops.Performance.Benchmarks.Updates;

[FastJob(RuntimeMoniker.Net70, unrollFactor: 1, maxIterationCount: 20, nugetPackages: "Microsoft.EntityFrameworkCore.Sqlite, 6.0.20", baseline: true)]
[FastJob(RuntimeMoniker.Net70, unrollFactor: 1, maxIterationCount: 20, nugetPackages: "Microsoft.EntityFrameworkCore.Sqlite, 7.0.9")]
public class EFCoreUpdates_Insertion
{
    private readonly SqliteConnection _connection;
    private readonly SimpleDbContext _context = null!;
    private readonly DataEntry[] _data = Enumerable.Range(1,2000).Select(x => new DataEntry() { Id = x, Value = x * 2 }).ToArray();


    public EFCoreUpdates_Insertion()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new SimpleDbContext(new DbContextOptionsBuilder().UseSqlite(_connection).EnableSensitiveDataLogging().Options);
        _context.Database.EnsureCreated();
    }

    [Benchmark]
    public void Insertion()
    {
        _context.AddRange(_data);
        _context.SaveChanges();
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

[FastJob(RuntimeMoniker.Net70, nugetPackages: "Microsoft.EntityFrameworkCore.Sqlite, 6.0.20", baseline: true)]
[FastJob(RuntimeMoniker.Net70, nugetPackages: "Microsoft.EntityFrameworkCore.Sqlite, 7.0.9")]
public class EFCoreUpdates_Reading
{
    private readonly SqliteConnection _connection;
    private readonly SimpleDbContext _context = null!;
    private readonly DataEntry[] _data = Enumerable.Range(1, 2000).Select(x => new DataEntry() { Id = x, Value = x * 2 }).ToArray();


    public EFCoreUpdates_Reading()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        _context = new SimpleDbContext(new DbContextOptionsBuilder().UseSqlite(_connection).EnableSensitiveDataLogging().Options);
        _context.Database.EnsureCreated();
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
        _context.SaveChanges();
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
        modelBuilder.Entity<DataEntry>().ToTable(nameof(DataEntries)).HasKey(x => x.Id);
        modelBuilder.Entity<DataEntry>().Property(x => x.Id).ValueGeneratedNever();
        base.OnModelCreating(modelBuilder);
    }
}

public class DataEntry
{
    public int Id { get; set; }

    public int Value { get; set; }
}
