using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskPrioritizationAPI.Core.Contracts;
using TaskPrioritizationAPI.Core.Services;
using TaskPrioritizationAPI.Infrastructure.Data;
using TaskPrioritizationAPI.Infrastructure.Data.Repositories;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbRepository, ApplicationDbRepository>();
            services.AddScoped<ITaskService, TaskService>();

            return services;
        }

        public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<Context>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}