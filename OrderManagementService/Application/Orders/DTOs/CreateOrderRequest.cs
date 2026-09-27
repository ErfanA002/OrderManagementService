namespace OrderManagementService.Application.Orders.DTOs;

public sealed record CreateOrderRequest(Guid CustomerId, List<CreateOrderItemRequest> Items);