using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args); 

var meter = new Meter("MyLibrary", "1.0");
builder.Services.AddOpenTelemetry()
    .WithMetrics(builder =>
    {
        builder
            .AddMeter(meter.Name)
            .AddPrometheusExporter();
    });

var app = builder.Build();

var counter = meter.CreateCounter<long>(
    "hello.requests.count",
    description: "The number of hello requests");

app.MapGet("/hello", async () =>
{
    counter.Add(1);

    await Task.Delay(Random.Shared.Next(1000));
    return "Hello, World!";
});

app.UseOpenTelemetryPrometheusScrapingEndpoint("metrics");

app.Run();
