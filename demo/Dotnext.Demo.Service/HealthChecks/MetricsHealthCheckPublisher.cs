using Dotnext.Demo.Service.Metrics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.Metrics;

namespace Dotnext.Demo.Service.HealthChecks;

public class MetricsHealthCheckPublisher : IHealthCheckPublisher
{
    private readonly OtelMetrics _otelMetrics;
    public MetricsHealthCheckPublisher(OtelMetrics otelMetrics) => _otelMetrics = otelMetrics;

    public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
    {
        foreach (var entry in report.Entries)
        {
            var tag = new KeyValuePair<string, object?>("name", entry.Key);
            _otelMetrics.Meter.CreateObservableGauge("health_check",
                () => new Measurement<int>((int)entry.Value.Status, tag),
                description: "Health check status");
        }

        _otelMetrics.Meter.CreateObservableGauge("health", 
            () => new Measurement<int>((int)report.Status),
            description: "General application health status");

        return Task.CompletedTask;
    }
}
