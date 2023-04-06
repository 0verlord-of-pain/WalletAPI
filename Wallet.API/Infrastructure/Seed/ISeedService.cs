namespace Wallet.API.Infrastructure.Seed;

internal interface ISeedService
{
    Task SeedRolesAsync();
    Task SeedAdminAndManagerAsync();
    Task SeedDefaultUserAsync();
}