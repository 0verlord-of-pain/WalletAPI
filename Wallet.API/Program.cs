using MediatR;
using System.Reflection;
using System.Text.Json.Serialization;
using Wallet.API.Infrastructure.Extensions;
using Wallet.API.Infrastructure.Seed;
using Wallet.Application;

namespace Wallet.API;
public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        services.AddDbContextsCustom(builder.Configuration);
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddIdentityCustom();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<ISeedService, SeedService>();
        services.AddAuthenticationCustom();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => { c.SchemaFilter<EnumSchemaFilterExtension>(); });

        services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(AssemblyInfo).GetTypeInfo().Assembly);

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseErrorHandler(builder.Environment);
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        try
        {
            SeedData.EnsureSeedData(app.Services).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }

        app.Run();
    }
}