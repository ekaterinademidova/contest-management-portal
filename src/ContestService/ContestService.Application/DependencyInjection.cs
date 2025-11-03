using ContestService.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ContestService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IEventStageService, EventStageService>();
        services.AddScoped<IEventStageCriteriaService, EventStageCriteriaService>();
        services.AddScoped<IContestNoticeService, ContestNoticeService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IContestDocsPackageService, ContestDocsPackageService>();

        // Validators - Register all validators from the Application assembly
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}

