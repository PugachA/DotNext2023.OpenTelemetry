using Dotnext.Demo.Core.Domain;

namespace Dotnext.Demo.Core.Abstractions;
public interface IMessageHandler<TMessage>
    where TMessage : IMessage
{
    Task Handle(TMessage message, CancellationToken ct);
}
