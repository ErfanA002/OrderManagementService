using OrderManagementService.Application.Orders.DTOs;
using OrderManagementService.Domain.Entities;

namespace OrderManagementService.Application.Orders.Services.Ports;

public interface IOrderService
{
    Task<OrderDto> CreateAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken = default);

    Task<OrderDto?> GetByIdAsync(
        int orderId,
        CancellationToken cancellationToken = default);

    Task<PagedResult<OrderDto>> SearchAsync(
        OrderSearchRequest request,
        CancellationToken cancellationToken = default);

    Task<OrderDto> ChangeStatusAsync(
        int orderId,
        OrderStatus newStatus,
        CancellationToken cancellationToken = default);

    Task BulkInsertAsync(
        IReadOnlyCollection<CreateOrderRequest> requests,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int orderId,
        CancellationToken cancellationToken = default);
}