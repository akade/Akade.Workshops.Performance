using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace Akade.Workshops.Performance.Api.Infrastructure;

public class NetworkLatencySimulatorInterceptor : DbCommandInterceptor
{
    private readonly int _initialDelayInMs;
    private readonly int _nextRecordDelayInMs;
    private readonly double _nextRecordDelayProbability;
    private DbTransaction? transaction;


    public NetworkLatencySimulatorInterceptor(int initialDelayInMs = 30, int nextRecordDelayInMs = 1, double nextRecordDelayProbability = 0.02)
    {
        _initialDelayInMs = initialDelayInMs;
        _nextRecordDelayInMs = nextRecordDelayInMs;
        _nextRecordDelayProbability = nextRecordDelayProbability;
    }

    public override async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        if (command.Transaction is null || command.Transaction != transaction)
        {
            await Task.Delay(_initialDelayInMs, cancellationToken);
            transaction = command.Transaction;
        }
        return new NetworkLatencySimulatingDataReader(_nextRecordDelayInMs, _nextRecordDelayProbability, await base.ReaderExecutedAsync(command, eventData, result, cancellationToken));
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        if (command.Transaction is null || command.Transaction != transaction)
        {
            Thread.Sleep(_initialDelayInMs);
            transaction = command.Transaction;
        }
        return new NetworkLatencySimulatingDataReader(_nextRecordDelayInMs, _nextRecordDelayProbability, base.ReaderExecuted(command, eventData, result));
    }
}
