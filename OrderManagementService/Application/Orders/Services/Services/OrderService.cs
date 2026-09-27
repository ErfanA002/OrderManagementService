using Microsoft.EntityFrameworkCore;
using OrderManagementService.Application.Orders.DTOs;
using OrderManagementService.Application.Orders.Services.Ports;
using OrderManagementService.Domain.Entities;
using OrderManagementService.Infrastructure.Persistance;

namespace OrderManagementService.Application.Orders.Services.Services;

public class OrderService(
    ApplicationDbContext dbContext) : IOrderService
{
    public async Task<OrderDto> CreateAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Items is null || request.Items.Count == 0)
        {
            throw new ArgumentException(
                "Order must contain at least one item.");
        }

        var order = new Order
        {
            CustomerId = request.CustomerId,
            CreatedAt = DateTimeOffset.UtcNow,
            Status = OrderStatus.Pending,
            Items = request.Items
                .Select(x => new OrderItem
                {
                    Name = x.Name
                })
                .ToList()
        };

        dbContext.Orders.Add(order);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(order);
    }

    public async Task<OrderDto?> GetByIdAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(
                x => x.Id == orderId,
                cancellationToken);

        return order is null
            ? null
            : Map(order);
    }

    public async Task<PagedResult<OrderDto>> SearchAsync(
        OrderSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = request.PageNumber < 1
            ? 1
            : request.PageNumber;

        var pageSize = request.PageSize switch
        {
            < 1 => 20,
            > 100 => 100,
            _ => request.PageSize
        };

        IQueryable<Order> query = dbContext.Orders
            .AsNoTracking();

        if (request.CustomerId.HasValue)
        {
            query = query.Where(x =>
                x.CustomerId == request.CustomerId.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x =>
                x.Status == request.Status.Value);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(x =>
                x.CreatedAt >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x =>
                x.CreatedAt <= request.ToDate.Value);
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var orders = await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Include(x => x.Items)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = orders
            .Select(Map)
            .ToList();

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        return new PagedResult<OrderDto>(
            items,
            pageNumber,
            pageSize,
            totalCount,
            totalPages);
    }

    public async Task<OrderDto> ChangeStatusAsync(
        int orderId,
        OrderStatus newStatus,
        CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(
                x => x.Id == orderId,
                cancellationToken);

        if (order is null)
        {
            throw new KeyNotFoundException(
                $"Order with id '{orderId}' was not found.");
        }

        order.ChangeStatus(newStatus);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(order);
    }

    public async Task BulkInsertAsync(
        IReadOnlyCollection<CreateOrderRequest> requests,
        CancellationToken cancellationToken = default)
    {
        if (requests.Count == 0)
        {
            return;
        }

        var orders = requests.Select(request =>
        {
            if (request.Items is null ||
                request.Items.Count == 0)
            {
                throw new ArgumentException(
                    "Every order must contain at least one item.");
            }

            return new Order
            {
                CustomerId = request.CustomerId,
                Status = OrderStatus.Pending,
                CreatedAt = DateTimeOffset.UtcNow,

                Items = request.Items
                    .Select(item => new OrderItem
                    {
                        Name = item.Name
                    })
                    .ToList()
            };
        }).ToList();

        await dbContext.Orders.AddRangeAsync(
            orders,
            cancellationToken);

        await dbContext.SaveChangesAsync(
            cancellationToken);
    }

    public async Task DeleteAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .FirstOrDefaultAsync(
                x => x.Id == orderId,
                cancellationToken);

        if (order is null)
        {
            throw new KeyNotFoundException(
                $"Order with id '{orderId}' was not found.");
        }

        dbContext.Orders.Remove(order);

        await dbContext.SaveChangesAsync(
            cancellationToken);
    }

    private static OrderDto Map(Order order)
    {
        return new OrderDto(
            order.Id,
            order.CustomerId,
            order.Status,
            order.CreatedAt,
            order.Items
                .Select(x => new OrderItemDto(
                    x.Id,
                    x.Name))
                .ToList());
    }
}