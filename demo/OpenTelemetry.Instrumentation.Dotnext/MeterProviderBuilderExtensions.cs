using OpenTelemetry.Metrics;

namespace OpenTelemetry.Instrumentation.Dotnext;

public static class MeterProviderBuilderExtensions
{
    public static MeterProviderBuilder AddUpTimeInstrumentation(
        this MeterProviderBuilder builder)
    {
        builder.AddMeter(UpTimeMetrics.InstrumentationName);
        builder.AddInstrumentation(new UpTimeMetrics());

        return builder;
    }

    public static MeterProviderBuilder AddMemoryCacheInstrumentation(
        this MeterProviderBuilder builder)
    {
        builder.AddMeter(MemoryCacheMetrics.InstrumentationName);
        builder.AddInstrumentation<MemoryCacheMetrics>();

        return builder;
    }
}
