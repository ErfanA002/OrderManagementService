using OrderManagementService.Domain.Entities;

namespace OrderManagementService.Application.Orders.DTOs;

public sealed record ChangeOrderStatusRequest(OrderStatus Status);