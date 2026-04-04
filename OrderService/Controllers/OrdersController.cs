using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Services;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderCommandService _commandService;
        private readonly IOrderQueryService _queryService;

        public OrdersController(IOrderCommandService commandService, IOrderQueryService queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] OrderDto request)
        {
            var order = await _commandService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<OrderDto>> Cancel(string id)
        {
            var order = await _commandService.CancelOrderAsync(id);
            if (order == null) return NotFound();
            return order;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(string id)
        {
            var order = await _queryService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return order;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> List(
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