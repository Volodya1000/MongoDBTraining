using MongoDBTraining.Application.Features.MovieFeatures.GetAll;

namespace MongoDBTraining.WebApi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureSevices(this IServiceCollection services)
    {
        var assemblies = typeof(GetAllMoviesHandler).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

      

        return services;
    }
}
