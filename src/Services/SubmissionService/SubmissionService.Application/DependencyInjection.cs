using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SubmissionService.Application.Services;

namespace SubmissionService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Services - use fully qualified name to avoid namespace conflict
        services.AddScoped<ISubmissionService, SubmissionService.Application.Services.SubmissionService>();

        // Validators - Register all validators from the Application assembly
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}

