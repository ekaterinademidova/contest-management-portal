using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubmissionService.Domain.Interfaces;
using SubmissionService.Infrastructure.Data;
using SubmissionService.Infrastructure.Repositories;

namespace SubmissionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<SubmissionDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(SubmissionDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();
        services.AddScoped<IAppealRepository, AppealRepository>();

        return services;
    }
}

