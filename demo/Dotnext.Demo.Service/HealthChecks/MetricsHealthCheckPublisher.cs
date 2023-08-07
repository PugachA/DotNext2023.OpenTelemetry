using Dotnext.Demo.Service.Metrics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
            _otelMetrics.HealthCheckGauge.SetValue((long)entry.Value.Status, tag);
        }

        _otelMetrics.HealthGauge.SetValue((long)report.Status);

        return Task.CompletedTask;
    }
}
