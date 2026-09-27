using OrderManagementService.Domain.Entities;

namespace OrderManagementService.Application.Orders.DTOs;

public sealed record OrderDto(
    int Id,
    Guid CustomerId,
    OrderStatus Status,
    DateTimeOffset CreatedAt,
    IReadOnlyList<OrderItemDto> Items);

public sealed record OrderItemDto(
    int Id,
    string Name);