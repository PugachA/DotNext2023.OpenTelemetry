using AutoMapper;
using Dotnext.Demo.Core.Abstractions;
using Dotnext.Demo.Core.Domain;
using Dotnext.Demo.Service.Metrics;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Dotnext.Demo.Service.Handlers;

public class MessageHandlerWithMetrics<TMessage, TEntity> : IMessageHandler<TMessage> 
    where TEntity : IEntity
    where TMessage : IMessage
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<MessageHandlerWithMetrics<TMessage, TEntity>> _logger;
    private readonly KeyValuePair<string, object?> _tag;
    private readonly Counter<long> _failCounter;
    private readonly Counter<long> _successCounter;
    private readonly Histogram<long> _latencyHistogram;
    private long _durationMs;

    public MessageHandlerWithMetrics(IRepository<TEntity> repository, IMapper mapper,
        ILogger<MessageHandlerWithMetrics<TMessage, TEntity>> logger, OtelMetrics otelMetrics)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;

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
            "handler.duration",() => new Measurement<long>(_durationMs, _tag),
            "ms", "Handle duration milliseconds");
    }

    public async Task Handle(TMessage message, CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.LogInformation($"Handle message {typeof(TMessage).Name}");
            var entity = _mapper.Map<TEntity>(message);
            await _repository.MergeEntity(entity, ct);

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
