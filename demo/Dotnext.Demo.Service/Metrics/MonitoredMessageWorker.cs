using Dotnext.Demo.Core.Abstractions;
using Dotnext.Demo.Core.Domain;
using System.Diagnostics;

namespace Dotnext.Demo.Service.Metrics;

public class MonitoredMessageWorker<TMessage, TEntity> : IMessageWorker<TMessage, TEntity>
    where TEntity : IEntity
    where TMessage : IMessage
{
    private readonly IMessageWorker<TMessage, TEntity> _messageWorker;
    private readonly OtelMetrics _otelMetrics;
    private readonly KeyValuePair<string, object?> _tag;

    public MonitoredMessageWorker(
        IMessageWorker<TMessage, TEntity> messageWorker,
        OtelMetrics otelMetrics)
    {
        _messageWorker = messageWorker;
        _otelMetrics = otelMetrics;
        _tag = new KeyValuePair<string, object?>("message", typeof(TMessage).Name);

        _otelMetrics.WorkerSuccessCounter.Add(0, _tag);
        _otelMetrics.WorkerFailCounter.Add(0, _tag);
    }

    public async Task RunAsync(CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _messageWorker.RunAsync(ct);
            _otelMetrics.WorkerSuccessCounter.Add(1, _tag);
        }
        catch (Exception)
        {
            _otelMetrics.WorkerFailCounter.Add(1, _tag);
            throw;
        }
        finally
        {
            _otelMetrics.WorkerDurationGauge.SetValue(stopwatch.ElapsedMilliseconds, _tag);
        }
    }
}
