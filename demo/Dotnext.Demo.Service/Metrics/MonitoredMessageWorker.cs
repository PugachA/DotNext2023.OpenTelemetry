using Dotnext.Demo.Core.Abstractions;
using Dotnext.Demo.Core.Domain;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Dotnext.Demo.Service.Metrics;

public class MonitoredMessageWorker<TMessage, TEntity> : IMessageWorker<TMessage, TEntity>
    where TEntity : IEntity
    where TMessage : IMessage
{
    private readonly IMessageWorker<TMessage, TEntity> _messageWorker;
    private readonly Counter<long> _successCounter;
    private readonly Counter<long> _failCounter;
    private readonly KeyValuePair<string, object?> _tag;

    private long _durationMs = 0;

    public MonitoredMessageWorker(
        IMessageWorker<TMessage, TEntity> messageWorker,
        OtelMetrics otelMetrics)
    {
        _messageWorker = messageWorker;
        _tag = new KeyValuePair<string, object?>("message", typeof(TMessage).Name);
        
        _successCounter = otelMetrics.Meter.CreateCounter<long>("worker.success", "count", "The number of worker success execution");
        _successCounter.Add(0, _tag);
        
        _failCounter = otelMetrics.Meter.CreateCounter<long>("worker.fail", "count", "The number of worker fail execution");
        _failCounter.Add(0, _tag);
        
        var measurement = new Measurement<long>(_durationMs, _tag);
        otelMetrics.Meter.CreateObservableGauge("worker.duration", () => measurement, "ms", "Worker run duration milliseconds");
    }

    public async Task RunAsync(CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _messageWorker.RunAsync(ct);
            _successCounter.Add(1, _tag);
        }
        catch (Exception)
        {
            _failCounter.Add(1, _tag);
            throw;
        }
        finally
        {
            _durationMs = stopwatch.ElapsedMilliseconds;
        }
    }
}
