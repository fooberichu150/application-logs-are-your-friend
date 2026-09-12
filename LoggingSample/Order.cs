namespace LoggingSample;

public sealed class Order
{
    public int Id { get; init; }
    public required string CustomerName { get; init; }

    [Sensitive]
    public required string Email { get; init; }

    [Sensitive]
    public required string CreditCardNumber { get; init; }

    public decimal Amount { get; init; }
}
