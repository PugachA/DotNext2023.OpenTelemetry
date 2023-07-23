using OpenTelemetry.Resources;
using System.Reflection;

namespace Dotnext.Demo.Service;
public class ResourceBuilderHelper
{
    public static ResourceBuilder CreateResourceBuilder(Type assemblyTag)
    {
        var assembly = Assembly.GetAssembly(assemblyTag)!.GetName();
        var resourceBuilder = ResourceBuilder
                .CreateDefault()
                .AddService(
                    serviceName: assembly.Name!,
                    serviceVersion: assembly.Version?.ToString(),
                    autoGenerateServiceInstanceId: false,
                    serviceInstanceId: Environment.MachineName);

        resourceBuilder.AddAttributes(new Dictionary<string, object>
        {
            ["machine.name"] = Environment.MachineName
        });

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environment is not null)
            resourceBuilder.AddAttributes(new Dictionary<string, object>
            {
                ["environment.name"] = environment
            });

        return resourceBuilder;
    }
}
