namespace LoggingSample;

// Mark any property that should never reach a sink in plain text.
[AttributeUsage(AttributeTargets.Property)]
public sealed class SensitiveAttribute : Attribute
{
}
