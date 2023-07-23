using System.Diagnostics;

namespace OpenTelemetry.Instrumentation.Dotnext;

public class UpTimeService
{
    private static readonly DateTime StartTime;

    static UpTimeService()
    {
        using var process = Process.GetCurrentProcess();
        StartTime = process.StartTime;
    }

    public static TimeSpan UpTime => DateTime.Now - StartTime;
}