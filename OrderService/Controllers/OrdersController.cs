using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Services;

namespace OrderService.API.Controllers
{
    /// <summary>REST API for managing orders.</summary>
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

        /// <summary>Creates a new order.</summary>
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] OrderDto request)
        {
            var order = await _commandService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        /// <summary>Cancels an existing order.</summary>
        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<OrderDto>> Cancel(string id)
        {
            var order = await _commandService.CancelOrderAsync(id);
            if (order == null) return NotFound();
            return order;
        }

        /// <summary>Gets an order by id.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(string id)
        {
            var order = await _queryService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return order;
        }

        /// <summary>Lists orders with optional filtering and pagination.</summary>
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