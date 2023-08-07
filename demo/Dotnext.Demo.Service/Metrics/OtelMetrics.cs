using System.Diagnostics.Metrics;

namespace Dotnext.Demo.Service.Metrics;

public class OtelMetrics : IDisposable
{
    public static readonly string MeterName = typeof(OtelMetrics).FullName!;
    private readonly Meter _meter;

    public OtelMetrics()
    {
        var version = typeof(OtelMetrics).Assembly
            .GetName().Version?.ToString();
        _meter = new Meter(MeterName, version);

        HandlerSuccessCounter = _meter.CreateCounter<long>(
            "handler.success", "count", "The number of handler success execution");

        HandlerFailCounter = _meter.CreateCounter<long>(
            "handler.fail", "count", "The number of handler fail execution");

        HandlerLatencyHistogram = _meter.CreateHistogram<long>(
            "handler.latency", "ms", "Message latency");

        HandlerDurationGauge = new Gauge<long>(
            _meter, "handler.duration", "ms", "Handle duration milliseconds");

        WorkerSuccessCounter = _meter.CreateCounter<long>(
            "worker.success", "count", "The number of worker success execution");

        WorkerFailCounter = _meter.CreateCounter<long>(
            "worker.fail", "count", "The number of worker fail execution");

        WorkerDurationGauge = new Gauge<long>(
            _meter, "worker.duration", "ms", "Worker run duration milliseconds");

        HealthCheckGauge = new Gauge<long>(
            _meter, "health.check", description: "Health check status");

        HealthGauge = new Gauge<long>(
            _meter, "health", description: "General application health status");
    }

    public Counter<long> HandlerSuccessCounter { get; }
    public Counter<long> HandlerFailCounter { get; }
    public Histogram<long> HandlerLatencyHistogram { get; }
    public Gauge<long> HandlerDurationGauge { get; }
    public Counter<long> WorkerSuccessCounter { get; }
    public Counter<long> WorkerFailCounter { get; }
    public Gauge<long> WorkerDurationGauge { get; }
    public Gauge<long> HealthCheckGauge { get; }
    public Gauge<long> HealthGauge { get; }

    public void Dispose() => _meter.Dispose();
}
