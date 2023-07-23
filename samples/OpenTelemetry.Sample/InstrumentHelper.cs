using System.Diagnostics.Metrics;

namespace OpenTelemetry.Sample;

internal static class InstrumentHelper
{
    public static void RunCounter(Meter meter)
    {
        Console.WriteLine("CreateCounter");

        var counter = meter.CreateCounter<long>(
            "counter.name",
            unit: "things",
            description: "A count of things");

        while (!Console.KeyAvailable)
        {
            counter.Add(200, new KeyValuePair<string, object?>("tag1", "value1"));
        }

        Console.ReadLine();
    }

    public static void RunObservableCounter(Meter meter)
    {
        Console.WriteLine("CreateObservableCounter");

        var observableCounter = meter.CreateObservableCounter(
            "observable.counter.name",
            () => new Measurement<long>(
                DateTime.UtcNow.Ticks,
                new KeyValuePair<string, object?>("tag2", "value2")),
            unit: "ticks",
            description: "DateTime.UtcNow.Ticks");

        Console.ReadLine();
    }

    public static void RunObservableGauge(Meter meter)
    {
        Console.WriteLine("CreateObservableGauge");

        var observableGauge = meter.CreateObservableGauge(
            "observable.gauge.name",
            () => new Measurement<long>(
                Random.Shared.Next(),
                new KeyValuePair<string, object?>("tag3", "value3")),
            unit: "unit",
            description: "Random.Shared.Next()");

        Console.ReadLine();
    }

    public static void RunHistogram(Meter meter)
    {
        Console.WriteLine("CreateHistogram");

        var histogram = meter.CreateHistogram<long>(
            "histogram.name",
            unit: "ms",
            description: "Random.Shared.Next(0, 1000)");

        while (!Console.KeyAvailable)
        {
            histogram.Record(
                Random.Shared.Next(0, 1000),
                new KeyValuePair<string, object?>("tag4", "value4"));
        }

        Console.ReadLine();
    }
}
