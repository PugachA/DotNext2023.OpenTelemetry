using System.Diagnostics.Metrics;

namespace Dotnext.Demo.Service.Metrics;

public class Gauge<T> where T : struct
{
    private readonly object _lock = new();
    private readonly Dictionary<string, Measurement<T>> _measurements = new();

    public Gauge(Meter meter, string name, string? unit = null, string? description = null)
    {
        meter.CreateObservableGauge(name, () => _measurements.Values, unit, description);
    }

    public void SetValue(T value, KeyValuePair<string, object?>? tag = null)
    {
        lock (_lock)
        {
            var key = tag.ToString() ?? string.Empty;
            var tags = tag is null ?
                    Array.Empty<KeyValuePair<string, object?>>() :
                    new[] { tag.Value };

            if (_measurements.ContainsKey(key))
                _measurements[key] = new (value, tags);
            else
                _measurements.Add(key, new(value, tags));
        }
    }
}
