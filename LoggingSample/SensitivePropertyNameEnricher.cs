using Serilog.Core;
using Serilog.Events;

namespace LoggingSample;

// Defense in depth for properties that never went through a model, e.g.
// Log.Information("Login attempt for {Password}", password). The
// destructuring policy above can't catch this because there's no [Sensitive]
// attribute to read; this enricher catches it by property name instead.
public sealed class SensitivePropertyNameEnricher : ILogEventEnricher
{
    private static readonly string[] DeniedNames =
    {
        "password", "ssn", "creditcardnumber", "email", "apikey", "token"
    };

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        foreach (var name in logEvent.Properties.Keys.ToList())
        {
            if (DeniedNames.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(name, "***REDACTED***"));
            }
        }
    }
}
