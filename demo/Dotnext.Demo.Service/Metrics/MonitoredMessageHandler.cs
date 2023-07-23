using Dotnext.Demo.Core.Abstractions;
using Dotnext.Demo.Core.Domain;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Dotnext.Demo.Service.Metrics;

public class MonitoredMessageHandler<TMessage> : IMessageHandler<TMessage>
    where TMessage : IMessage
{
    private readonly IMessageHandler<TMessage> _messageHandler;
    private readonly KeyValuePair<string, object?> _tag;
    private readonly Counter<long> _successCounter;
    private readonly Counter<long> _failCounter;
    private readonly Histogram<long> _latencyHistogram;
    private long _durationMs;

    public MonitoredMessageHandler(IMessageHandler<TMessage> messageHandler,
        OtelMetrics otelMetrics)
    {
        _messageHandler = messageHandler;
        _tag = new KeyValuePair<string, object?>("message", typeof(TMessage).Name);

        _successCounter = otelMetrics.Meter.CreateCounter<long>(
            "handler.success", "count", "The number of handler success execution");
        _successCounter.Add(0, _tag);

        _failCounter = otelMetrics.Meter.CreateCounter<long>(
            "handler.fail", "count", "The number of handler fail execution");
        _failCounter.Add(0, _tag);

        _latencyHistogram = otelMetrics.Meter.CreateHistogram<long>(
            "handler.latency", "ms", "Message latency");

        otelMetrics.Meter.CreateObservableGauge(
            "handler.duration", () => new Measurement<long>(_durationMs, _tag),
            "ms", "Handle duration milliseconds");
    }

    public async Task Handle(TMessage message, CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _messageHandler.Handle(message, ct);
            _successCounter.Add(1, _tag);

            _latencyHistogram.Record(
                (long)(DateTime.UtcNow - message.TimestampUtc).TotalMilliseconds, _tag);
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
