using Dotnext.Demo.Core.Domain;

namespace Dotnext.Demo.Core.Abstractions;
public interface IMessageWorker<TMessage, TEntity> where TEntity : IEntity where TMessage : IMessage
{
    Task RunAsync(CancellationToken ct);
}
