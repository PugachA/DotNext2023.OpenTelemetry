using System.Diagnostics.Metrics;
using OpenTelemetry.Sample;

var meter = new Meter("MyLibrary", "1.0");
using var meterProvider = MeterProviderHelper.CreateConsoleMeterProvider(meter.Name);

InstrumentHelper.RunCounter(meter);
//InstrumentHelper.RunObservableGauge(meter);
//InstrumentHelper.RunHistogram(meter);