using Dotnext.Demo.Core.Abstractions;
using Dotnext.Demo.Core.Domain;
using System.Diagnostics;

namespace Dotnext.Demo.Service.Metrics;

public class MonitoredMessageHandler<TMessage> : IMessageHandler<TMessage>
    where TMessage : IMessage
{
    private readonly IMessageHandler<TMessage> _messageHandler;
    private readonly OtelMetrics _otelMetrics;
    private readonly KeyValuePair<string, object?> _tag;

    public MonitoredMessageHandler(IMessageHandler<TMessage> messageHandler,
        OtelMetrics otelMetrics)
    {
        _messageHandler = messageHandler;
        _otelMetrics = otelMetrics;
        _tag = new KeyValuePair<string, object?>("message", typeof(TMessage).Name);

        _otelMetrics.HandlerSuccessCounter.Add(0, _tag);
        _otelMetrics.HandlerFailCounter.Add(0, _tag);
    }

    public async Task Handle(TMessage message, CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _messageHandler.Handle(message, ct);
            _otelMetrics.HandlerSuccessCounter.Add(1, _tag);

            _otelMetrics.HandlerLatencyHistogram.Record(
                (long)(DateTime.UtcNow - message.TimestampUtc).TotalMilliseconds, _tag);
        }
        catch (Exception)
        {
            _otelMetrics.HandlerFailCounter.Add(1, _tag);
            throw;
        }
        finally
        {
            _otelMetrics.HandlerDurationGauge.SetValue(stopwatch.ElapsedMilliseconds, _tag);
        }
    }
}
