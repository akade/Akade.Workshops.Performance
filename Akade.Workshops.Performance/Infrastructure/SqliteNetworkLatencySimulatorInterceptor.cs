using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace Akade.Workshops.Performance.Api.Infrastructure;

public class SqliteNetworkLatencySimulatorInterceptor : DbCommandInterceptor
{
    private readonly int _initialDelayInMs;
    private readonly int _nextRecordDelayInMs;
    private readonly double _nextRecordDelayProbability;

    public SqliteNetworkLatencySimulatorInterceptor(int initialDelayInMs = 1, int nextRecordDelayInMs = 1, double nextRecordDelayProbability = 0.02)
    {
        _initialDelayInMs = initialDelayInMs;
        _nextRecordDelayInMs = nextRecordDelayInMs;
        _nextRecordDelayProbability = nextRecordDelayProbability;
    }

    public override async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        await Task.Delay(_initialDelayInMs, cancellationToken);
        return new RemoteLatencySimulatingDataReader(_nextRecordDelayInMs, _nextRecordDelayProbability, await base.ReaderExecutedAsync(command, eventData, result, cancellationToken));
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        Thread.Sleep(_initialDelayInMs);
        return new RemoteLatencySimulatingDataReader(_nextRecordDelayInMs, _nextRecordDelayProbability, base.ReaderExecuted(command, eventData, result));
    }
}
