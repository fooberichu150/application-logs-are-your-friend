using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace LoggingSample;

// Runs whenever an object is logged with the @ destructuring operator.
// Any property carrying [Sensitive] gets masked before it reaches a sink;
// everything else destructures normally. This is the recommended approach:
// mark the property once on the model and every log call is covered.
public sealed class SensitiveDataDestructuringPolicy : IDestructuringPolicy
{
    public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
    {
        var type = value.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var hasSensitiveProperty = properties.Any(p => p.GetCustomAttribute<SensitiveAttribute>() is not null);

        if (!hasSensitiveProperty)
        {
            result = null!;
            return false;
        }

        var members = new List<LogEventProperty>();
        foreach (var property in properties)
        {
            var isSensitive = property.GetCustomAttribute<SensitiveAttribute>() is not null;
            var propertyValue = isSensitive
                ? new ScalarValue("***REDACTED***")
                : propertyValueFactory.CreatePropertyValue(property.GetValue(value), destructureObjects: true);

            members.Add(new LogEventProperty(property.Name, propertyValue));
        }

        result = new StructureValue(members, type.Name);
        return true;
    }
}
