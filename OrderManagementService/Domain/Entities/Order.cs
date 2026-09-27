namespace OrderManagementService.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public Guid CustomerId { get; set; }

    public OrderStatus Status { get;  set; }

    public DateTimeOffset CreatedAt { get; set; }

    public List<OrderItem> Items { get; set; } = [];

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (newStatus != Status + 1)
        {
            throw new InvalidOperationException(
                $"Invalid order status transition from {Status} to {newStatus}.");
        }

        Status = newStatus;
    }
}

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Shipped = 2,
    Delivered = 3
}