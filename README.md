# Application Logs Are Your Friend (and How to Write Them)

Companion code for the blog post [Application Logs Are Your Friend (and How to Write Them)](https://www.seeleycoder.com/blog/application-logs-are-your-friend-and-how-to-write-them).

The post covers picking the right log level, why structured logging with [Serilog](https://serilog.net/) beats string concatenation, how to keep PII out of your logs, and how sinks route log events to their destinations. `LoggingSample` is a small .NET console app that puts all of that into working code: it simulates an order-processing flow logging across every level from Debug to Fatal, masks sensitive `Order` properties automatically via an attribute-driven `IDestructuringPolicy`, catches sensitive values logged outside a model with a property-name `ILogEventEnricher`, and fans events out to both a console sink and a rolling file sink at different minimum levels.

## Running it

```
cd LoggingSample
dotnet run
```
