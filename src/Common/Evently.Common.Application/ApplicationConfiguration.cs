using System.Reflection;
using Evently.Common.Application.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        Assembly[] moduleAssemblies)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(moduleAssemblies);

            config.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehaviour<,>));
            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehaviour<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehaviour<,>));
        });

        services.AddValidatorsFromAssemblies(moduleAssemblies, includeInternalTypes: true);

        return services;
    }
}
