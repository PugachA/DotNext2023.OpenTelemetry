using Dotnext.Demo.Core.Abstractions;
using Dotnext.Demo.Core.Domain;
using Dotnext.Demo.Service.Handlers;
using Dotnext.Demo.Service.HealthChecks;
using Dotnext.Demo.Service.Messages;
using Dotnext.Demo.Service.Metrics;
using Dotnext.Demo.Service.Workers;
using Dotnext.Demo.Service.Workers.Fakers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

namespace Dotnext.Demo.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageWorkers(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddTransient<IMessageFaker<UserMessage>, MessageFaker>();
        services.AddMessageWorker<UserMessage, User>();
        services.AddTransient<IMessageFaker<OrderMessage>, MessageFaker>();
        services.AddMessageWorker<OrderMessage, Order>();

        return services;
    }

    public static IServiceCollection AddMessageWorker<TMessage, TEntity>(this IServiceCollection services)
        where TEntity : IEntity
        where TMessage : IMessage
    {
        services.AddTransient<IRepository<TEntity>, InMemoryRepository<TEntity>>();
        services.AddTransient<IMessageHandler<TMessage>, MessageHandler<TMessage, TEntity>>();
        //services.AddTransient<IMessageHandler<TMessage>, MessageHandlerWithMetrics<TMessage, TEntity>>();
        services.AddTransient<IMessageWorker<TMessage, TEntity>, MessageWorker<TMessage, TEntity>>();
        services.AddHostedService<HostedService<TMessage, TEntity>>();
        return services;
    }

    public static IServiceCollection AddMonitoring(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<SampleHealthCheck>("sample");

        services.AddSingleton<
            IHealthCheckPublisher,
            MetricsHealthCheckPublisher>();

        services.Decorate(typeof(IMessageWorker<,>), typeof(MonitoredMessageWorker<,>));

        services.Decorate(
            typeof(IMessageHandler<>),
            typeof(MonitoredMessageHandler<>));

        return services;
    }
}
