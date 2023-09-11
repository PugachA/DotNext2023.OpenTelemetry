using System.Diagnostics.Metrics;

namespace OpenTelemetry.Instrumentation.Dotnext;

internal sealed class UpTimeMetrics : IDisposable
{
    internal static readonly string InstrumentationName = typeof(UpTimeMetrics).FullName!;
    internal static readonly string? InstrumentationVersion 
        = typeof(UpTimeMetrics).Assembly.GetName().Version?.ToString();

    private readonly Meter _meter;

    public UpTimeMetrics()
    {
        _meter = new Meter(InstrumentationName, InstrumentationVersion);
        var _ = _meter.CreateObservableCounter(
            "application.uptime",
            () => (long)UpTimeProvider.GetUpTime().TotalMilliseconds,
            unit: "ms",
            description: "Milliseconds elapsed since application startup");
    }

    public void Dispose() => _meter.Dispose();
}
