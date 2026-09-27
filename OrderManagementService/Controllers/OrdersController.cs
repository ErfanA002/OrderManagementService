using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementService.Application.Orders.DTOs;
using OrderManagementService.Application.Orders.Services.Ports;
using OrderManagementService.Infrastructure.Identity;

namespace OrderManagementService.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(
    IOrderService orderService) : ControllerBase
{
    [HttpPost]
    [HasPermission(Permission.CreateOrder)]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await orderService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpGet("{id:int}")]
    [HasPermission(Permission.OrderDetail)]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await orderService.GetByIdAsync(
            id,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    [HasPermission(Permission.SearchOrder)]
    public async Task<IActionResult> Search(
        [FromQuery] OrderSearchRequest request,
        CancellationToken cancellationToken)
    {
        var result = await orderService.SearchAsync(
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{id:int}/status")]
    [HasPermission(Permission.OrderChangeStatus)]
    public async Task<IActionResult> ChangeStatus(
        int id,
        [FromBody] ChangeOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await orderService.ChangeStatusAsync(
            id,
            request.Status,
            cancellationToken);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await orderService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("bulk")]
    [HasPermission(Permission.OrderBulkInsert)]
    public async Task<IActionResult> BulkInsert(
        [FromBody] List<CreateOrderRequest> requests,
        CancellationToken cancellationToken)
    {
        await orderService.BulkInsertAsync(
            requests,
            cancellationToken);

        return Ok();
    }
}