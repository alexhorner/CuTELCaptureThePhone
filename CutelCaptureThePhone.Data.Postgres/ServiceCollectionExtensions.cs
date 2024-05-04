using CutelCaptureThePhone.Core.Providers;
using CutelCaptureThePhone.Data.Postgres.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CutelCaptureThePhone.Data.Postgres
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCutelCaptureThePhonePostgres(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CutelCaptureThePhoneDbContext>(options =>
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