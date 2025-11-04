using ContestService.Domain.Interfaces;
using ContestService.Infrastructure.Data;
using ContestService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContestService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<ContestDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ContestDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEventStageRepository, EventStageRepository>();
        services.AddScoped<IEventStageCriteriaRepository, EventStageCriteriaRepository>();
        services.AddScoped<IContestNoticeRepository, ContestNoticeRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IContestDocsPackageRepository, ContestDocsPackageRepository>();

        return services;
    }
}

