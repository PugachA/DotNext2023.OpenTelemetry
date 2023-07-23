using System.Diagnostics.Metrics;

namespace Dotnext.Demo.Service.Metrics;

public class Gauge
{
	private long _value = 0;

	public Gauge(Meter meter)
	{
		var mes = new Measurement<long>(_value, null);

        meter.CreateObservableGauge("name", () => new Measurement<long>(_value, null));
	}

	public void SetValue(long value)
	{
        Interlocked.Exchange(ref _value, value);
	}
}
