using CutelPhoneGame.Core.Providers;
using CutelPhoneGame.Data.Postgres.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CutelPhoneGame.Data.Postgres
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCutelPhoneGamePostgres(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CutelPhoneGameDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services
                .AddScoped<IUserProvider, PostgresUserProvider>()
                .AddScoped<IPlayerProvider, PostgresPlayerProvider>()
                .AddScoped<ICaptureProvider, PostgresCaptureProvider>()
                .AddScoped<IBlacklistProvider, PostgresBlacklistProvider>()
                .AddScoped<IWhitelistProvider, PostgresWhitelistProvider>();

            return services;
        }
    }
}