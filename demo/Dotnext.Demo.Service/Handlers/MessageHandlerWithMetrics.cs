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
    private readonly OtelMetrics _otelMetrics;
    private readonly KeyValuePair<string, object?> _tag;

    public MessageHandlerWithMetrics(IRepository<TEntity> repository, IMapper mapper,
        ILogger<MessageHandlerWithMetrics<TMessage, TEntity>> logger, OtelMetrics otelMetrics)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
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
            _logger.LogInformation($"Handle message {typeof(TMessage).Name}");
            var entity = _mapper.Map<TEntity>(message);
            await _repository.MergeEntity(entity, ct);

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

