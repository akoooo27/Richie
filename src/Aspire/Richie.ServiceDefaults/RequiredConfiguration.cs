using Microsoft.Extensions.Configuration;

namespace Richie.ServiceDefaults;

public sealed class RequiredConfiguration(IConfiguration configuration)
{
    private readonly List<string> _problems = [];

    public string Value(string key)
    {
        return Require(key, configuration[key]);
    }

    public string ConnectionString(string name)
    {
        return Require($"ConnectionStrings:{name}", configuration.GetConnectionString(name));
    }

    public string HttpsAddress(string key)
    {
        string value = Require(key, configuration[key]);

        if (value.Length == 0)
        {
            return value;
        }

        if (!Uri.TryCreate(value, UriKind.Absolute, out Uri? uri) || uri.Scheme != Uri.UriSchemeHttps)
        {
            _problems.Add($"'{key}' must be an absolute https URL, got '{value}'.");

            return string.Empty;
        }

        return value;
    }

    public void ThrowIfInvalid()
    {
        if (_problems.Count > 0)
        {
            throw new InvalidOperationException("Invalid configuration: " + string.Join(' ', _problems));
        }
    }

    private string Require(string key, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _problems.Add($"'{key}' is missing.");

            return string.Empty;
        }

        return value;
    }
}
