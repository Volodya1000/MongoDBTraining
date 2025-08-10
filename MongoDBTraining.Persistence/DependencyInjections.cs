using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDBTraining.Domain.Interfaces.Repositories;
using MongoDBTraining.Persistence.Repositories;
using MongoDBTraining.Persistence.Settings;

namespace MongoDBTraining.Persistence;

public static class DependencyInjections
{
    public static IServiceCollection ConfigurePersistence(this IServiceCollection services, 
                                                          IConfiguration configuration)
    {
        services
           .AddOptions<MongoSettings>()
           .Bind(configuration.GetSection(nameof(MongoSettings)))
           .ValidateOnStart();

        services.AddSingleton<IValidateOptions<MongoSettings>, MongoSettingsValidation>();

        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        services.AddScoped<ApplicationDbContext>();

        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IActorRepository, ActorRepository>();

        return services;
    }
}
