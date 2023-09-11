using Dotnext.Demo.Service;
using Dotnext.Demo.Service.Metrics;
using OpenTelemetry.Instrumentation.Dotnext;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMessageWorkers()
    .AddMonitoring();

builder.Services.AddOpenTelemetry()
    .WithMetrics(builder =>
    {
        builder
            .SetResourceBuilder(ResourceBuilderHelper.CreateResourceBuilder(typeof(Program)))
            .AddMeter(OtelMetrics.MeterName)
            .AddInstrumentation<OtelMetrics>()
            .AddUpTimeInstrumentation()
            .AddPrometheusExporter()
            .AddOtlpExporter(op =>
            {
                op.Endpoint = new Uri("http://localhost:4317");
            });
    });

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint("metrics");
app.MapHealthChecks("/health");

app.Run();