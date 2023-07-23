using OpenTelemetry.Metrics;

namespace OpenTelemetry.Sample;
internal static class MeterProviderHelper
{
    public static MeterProvider? CreateConsoleMeterProvider(string meterName)
    {
        return Sdk.CreateMeterProviderBuilder()
            .AddMeter(meterName)
            .AddConsoleExporter((_, op)
                => op.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000)
            .Build();
    }

    public static MeterProvider? CreatePrometheusMeterProvider(string meterName)
    {
        return Sdk.CreateMeterProviderBuilder()
            .AddMeter(meterName)
            .AddPrometheusHttpListener(opt =>
            {
                opt.UriPrefixes = new[] { "http://localhost:5000/" };
                opt.ScrapeEndpointPath = "/metrics";
            })
            .Build();
    }
}
