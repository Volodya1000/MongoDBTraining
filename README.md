## Работа с MongoDB
Реализован ApplicationDbContext — это класс, отвечающий за подключение и доступ к коллекциям MongoDB в проекте. Он создаёт и храняет объект базы данных (IMongoDatabase), который потом используется репозиториями.

Тоесть работа с MongoDB организована максимально схоже как с EF Core

## Конфигурация и валидация настроек MongoDB с использованием Options pattern в ASP.NET Core
В проекте для настройки подключения к MongoDB используется паттерн Options — стандартный способ работы с настройками в ASP.NET Core.

### Класс с настройками
Его свойства должны быть заполнены из конфигурационного файла.
```csharp
public class MongoSettings
{
    public required string ConnectionString { get; init; }
    public required string DatabaseName { get; init; }
}
```

### Валидация настроек
```csharp
Валидация реализована отдельным классом, реализующим интерфейс IValidateOptions<T>
public class MongoSettingsValidation : IValidateOptions<MongoSettings>
{
    public ValidateOptionsResult Validate(string? name, MongoSettings options)
    {
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
            return ValidateOptionsResult.Fail(
                $"MongoDB {nameof(MongoSettings.ConnectionString)} is missing in configuration");

        if (string.IsNullOrWhiteSpace(options.DatabaseName))
            return ValidateOptionsResult.Fail(
                $"MongoDB {nameof(MongoSettings.DatabaseName)} is missing in configuration");

        return ValidateOptionsResult.Success;
    }
}
```

### Регистрация опций и валидации

В классе расширения MongoDBTraining.Persistence.DependencyInjections настроена регистрация конфигурации

```csharp
public static IServiceCollection ConfigurePersistence(this IServiceCollection services, 
                                                      IConfiguration configuration)
{
    services
       .AddOptions<MongoSettings>()                                       // ① Создаём OptionsBuilder<MongoSettings>
       .Bind(configuration.GetSection(nameof(MongoSettings)))              // ② Привязываем секцию конфигурации MongoSettings из appsettings.json
       .ValidateOnStart();                                                  // ③ Запускаем валидацию при старте приложения

    services.AddSingleton<IValidateOptions<MongoSettings>, MongoSettingsValidation>(); 

    services.AddSingleton<IMongoClient>(sp =>
    {
        var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;  
        return new MongoClient(settings.ConnectionString);                      
    });

    services.AddScoped<ApplicationDbContext>();
    services.AddScoped<IMovieRepository, MovieRepository>();

    return services;
}
```