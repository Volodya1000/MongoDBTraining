using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDBTraining.Persistence.Settings;

namespace MongoDBTraining.Persistence;

public static class DependencyInjections
{
    public static IServiceCollection ConfigurePersistence(this IServiceCollection services, 
                                                          IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));

        services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<MongoSettings>>();
            var settings = options.Value 
            ?? throw new InvalidOperationException("MongoSettings configuration is missing");
            return new MongoClient(settings.ConnectionString);
        });

        services.AddScoped<ApplicationDbContext>();

        return services;
    }
}
