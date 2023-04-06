using Microsoft.AspNetCore.Identity;
using Wallet.Domain.Entities;
using Wallet.Storage.Persistence;

namespace Wallet.API.Infrastructure.Extensions;

public static class IdentityConfigurationExtensions
{
    public static IServiceCollection AddIdentityCustom(
        this IServiceCollection services)
    {
        services
            .AddIdentity<User, IdentityRole<Guid>>(ops =>
            {
                ops.User.RequireUniqueEmail = true;
                ops.Password.RequireDigit = false;
                ops.Password.RequireUppercase = false;
                ops.Password.RequireNonAlphanumeric = false;
                ops.Password.RequiredLength = 5;
            })
            .AddEntityFrameworkStores<DataContext>();

        return services;
    }
}