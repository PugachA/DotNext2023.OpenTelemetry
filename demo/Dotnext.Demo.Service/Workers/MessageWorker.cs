using Dotnext.Demo.Core.Abstractions;
using Dotnext.Demo.Core.Domain;
using Dotnext.Demo.Service.Workers.Fakers;

namespace Dotnext.Demo.Service.Workers;

public class MessageWorker<TMessage, TEntity> : IMessageWorker<TMessage, TEntity> 
    where TEntity : IEntity
    where TMessage : IMessage
{
    private readonly IMessageHandler<TMessage> _messageHandler;
    private readonly IMessageFaker<TMessage> _messageFaker;

    public MessageWorker(
        IMessageHandler<TMessage> messageHandler,
        IMessageFaker<TMessage> messageFaker)
    {
        _messageHandler = messageHandler;
        _messageFaker = messageFaker;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        while(ct.IsCancellationRequested is false)
        {
            var message = _messageFaker.GenerateMessage();
            await _messageHandler.Handle(message, ct);

            await Task.Delay(Random.Shared.Next(1000), ct);
        }
    }
}
