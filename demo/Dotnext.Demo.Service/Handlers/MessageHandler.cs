using AutoMapper;
using Dotnext.Demo.Core.Abstractions;
using Dotnext.Demo.Core.Domain;

namespace Dotnext.Demo.Service.Handlers;

public class MessageHandler<TMessage, TEntity> : IMessageHandler<TMessage>
    where TEntity : IEntity
    where TMessage : IMessage
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<MessageHandler<TMessage, TEntity>> _logger;

    public MessageHandler(IRepository<TEntity> repository, IMapper mapper,
        ILogger<MessageHandler<TMessage, TEntity>> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(TMessage message, CancellationToken ct)
    {
        _logger.LogInformation($"Handle message {typeof(TMessage).Name}");
        var entity = _mapper.Map<TEntity>(message);
        await _repository.MergeEntity(entity, ct);
    }
}
