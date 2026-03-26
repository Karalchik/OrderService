using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Services;
using OrderService.Domain.Models;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderCommandService _commandService;
        private readonly OrderQueryService _queryService;

        public OrdersController(OrderCommandService commandService, OrderQueryService queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }

        //write

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var order = await _commandService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id)
        {
            var order = await _commandService.CancelOrderAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        //read

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var order = await _queryService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> List(
            [FromQuery] string? userId,
            [FromQuery] string? status,
            [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            var orders = await _queryService.GetOrdersAsync(userId, status, limit, offset);
            return Ok(orders);
        }
    }
}