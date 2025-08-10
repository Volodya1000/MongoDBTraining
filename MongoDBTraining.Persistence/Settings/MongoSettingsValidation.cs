using Microsoft.Extensions.Options;

namespace MongoDBTraining.Persistence.Settings;

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
