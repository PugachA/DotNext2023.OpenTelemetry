using Dotnext.Demo.Core.Domain;

namespace Dotnext.Demo.Core.Abstractions;
public interface IRepository<TEntity> where TEntity : IEntity
{
    Task MergeEntity(TEntity entity, CancellationToken ct);
}
