using System.Diagnostics.Metrics;

namespace Dotnext.Demo.Service.Metrics;

public class OtelMetrics : IDisposable
{
    public static readonly string MeterName 
        = typeof(OtelMetrics).FullName!;
    public readonly Meter Meter;

    public OtelMetrics()
    {
        var version = typeof(OtelMetrics).Assembly
            .GetName().Version?.ToString();
        Meter = new Meter(MeterName, version);
    }

    public void Dispose() => Meter.Dispose();
}
