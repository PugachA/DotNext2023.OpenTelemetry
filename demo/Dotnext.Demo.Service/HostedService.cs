using Dotnext.Demo.Core.Abstractions;
using Dotnext.Demo.Core.Domain;

namespace Dotnext.Demo.Service;

public class HostedService<TMessage, TEntity> : BackgroundService
    where TEntity : IEntity
    where TMessage : IMessage
{
    private readonly TimeSpan _defaultDelay = TimeSpan.FromSeconds(10);
    private readonly IMessageWorker<TMessage, TEntity> _messageWorker;
    private readonly ILogger<HostedService<TMessage, TEntity>> _logger;

    public HostedService(IMessageWorker<TMessage, TEntity> messageWorker, ILogger<HostedService<TMessage, TEntity>> logger)
    {
        _messageWorker = messageWorker;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(1, stoppingToken);

        _logger.LogInformation("Start HostedService");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Start processing");
            try
            {
                await _messageWorker.RunAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing");
            }

            _logger.LogInformation("Stop processing");
            await Task.Delay(_defaultDelay, stoppingToken);
        }
    }
}
