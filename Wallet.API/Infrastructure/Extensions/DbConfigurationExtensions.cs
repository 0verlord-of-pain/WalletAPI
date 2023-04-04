using Microsoft.EntityFrameworkCore;
using Wallet.Storage.Persistence;

namespace Wallet.API.Infrastructure.Extensions;
public static class DbConfigurationExtensions
{
    public static IServiceCollection AddDbContextsCustom(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("DefaultConnection");

        services
            .AddDbContext<DataContext>(options => options.UseNpgsql(connection,
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));

        return services;
    }
}