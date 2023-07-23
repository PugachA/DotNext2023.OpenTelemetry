using System.Diagnostics.Metrics;
using Microsoft.Extensions.Caching.Memory;

namespace OpenTelemetry.Instrumentation.Dotnext;

internal sealed class MemoryCacheMetrics : IDisposable
{
    private static readonly Type Type = typeof(MemoryCacheMetrics);
    internal static readonly string InstrumentationName = Type.FullName!;
    internal static readonly string? InstrumentationVersion = Type.Assembly.GetName().Version?.ToString();

    private readonly Meter _meter;

    public MemoryCacheMetrics(IMemoryCache memoryCache)
    {
        _meter = new Meter(InstrumentationName, InstrumentationVersion);

        var statistics = memoryCache.GetCurrentStatistics();
        if (statistics is not null)
        {
            var entryCount = _meter.CreateObservableGauge(
                "memory.cache.statistic.entry.count",
                () => memoryCache.GetCurrentStatistics()!.CurrentEntryCount,
                description: "The number of current entries in memory cache");

            var totalHits = _meter.CreateObservableGauge(
                "memory.cache.statistic.total.hit.count",
                () => memoryCache.GetCurrentStatistics()!.TotalHits,
                description: "The number of total memory cache hits");

            var totalMisses = _meter.CreateObservableGauge(
                "memory.cache.statistic.total.miss.count",
                () => memoryCache.GetCurrentStatistics()!.TotalMisses,
                description: "The number of total memory cache misses");
        }
    }

    public void Dispose()
    {
        _meter.Dispose();
    }
}
