using LoggingSample;
using Serilog;
using Serilog.Events;

// Two sinks, one set of log calls: the console gets everything for local
// debugging, the rolling file only keeps Information and above, since that's
// what you'd actually want to search through later.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.With<SensitivePropertyNameEnricher>()
    .Destructure.With<SensitiveDataDestructuringPolicy>()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        restrictedToMinimumLevel: LogEventLevel.Information,
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var order = new Order
    {
        Id = 1042,
        CustomerName = "Jamie Rivera",
        Email = "jamie.rivera@example.com",
        CreditCardNumber = "4111-1111-1111-1111",
        Amount = 84.50m
    };

    Log.Debug("Received order request for customer {CustomerName}", order.CustomerName);

    // @ tells Serilog to destructure Order instead of calling ToString().
    // The [Sensitive] properties come out masked; everything else doesn't.
    Log.Information("Order {OrderId} accepted for {@Order}", order.Id, order);

    // Email isn't part of a destructured object here, just a loose property,
    // but the enricher still catches it by name and redacts it.
    Log.Information("Sending confirmation email to {Email} for order {OrderId}", order.Email, order.Id);

    const int remainingStock = 3;
    if (remainingStock < 5)
    {
        Log.Warning("Stock for order {OrderId} is low: {RemainingStock} left", order.Id, remainingStock);
    }

    try
    {
        ChargeCard(order);
    }
    catch (InvalidOperationException ex)
    {
        Log.Error(ex, "Payment failed for order {OrderId}", order.Id);
    }

    Log.Fatal("Payment provider unreachable, halting order processing");
}
finally
{
    Log.CloseAndFlush();
}

static void ChargeCard(Order order)
{
    throw new InvalidOperationException($"Card declined for order {order.Id}");
}
