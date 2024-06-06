using Microsoft.EntityFrameworkCore;
using NeofiliaApi.Data.Repository;
using NeofiliaDomain.Application.Common.Repositories;

namespace NeofiliaApi.CollectionExtensions
{
    public static class DataExtentions
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            //Add dbcontext
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITableRepository, TableRepository>();
            services.AddScoped<IRewardRepository, RewardRepository>();
            return services;
        }
    }
}
