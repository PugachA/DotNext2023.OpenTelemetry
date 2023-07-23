using Dotnext.Demo.Core.Abstractions;
using Dotnext.Demo.Core.Domain;
using System.Collections.Concurrent;

namespace Dotnext.Demo.Service;
public class InMemoryRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
{
    private ConcurrentDictionary<long, TEntity> _dataStorage;

    public InMemoryRepository()
    {
        _dataStorage = new ConcurrentDictionary<long, TEntity>();
    }

    public async Task MergeEntity(TEntity entity, CancellationToken ct)
    {
        _dataStorage.AddOrUpdate(entity.Id, (k) => entity, (k, e) => entity);

        await Task.Delay(Random.Shared.Next(1000), ct);
    }
}
