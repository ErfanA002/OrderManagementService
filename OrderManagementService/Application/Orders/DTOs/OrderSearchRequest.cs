using OrderManagementService.Domain.Entities;

namespace OrderManagementService.Application.Orders.DTOs;

public sealed record OrderSearchRequest(
    Guid? CustomerId = null,
    OrderStatus? Status = null,
    DateTimeOffset? FromDate = null,
    DateTimeOffset? ToDate = null,
    int PageNumber = 1,
    int PageSize = 20);