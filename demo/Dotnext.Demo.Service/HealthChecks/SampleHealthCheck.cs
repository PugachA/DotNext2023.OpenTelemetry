using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Dotnext.Demo.Service.HealthChecks;

public class SampleHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var isHealthy = Random.Shared.Next(2) == 1;

        if (isHealthy)
            return Task.FromResult(
                HealthCheckResult.Healthy("A healthy result."));

        return Task.FromResult(
            new HealthCheckResult(context.Registration.FailureStatus,
            "An unhealthy result."));
    }
}
