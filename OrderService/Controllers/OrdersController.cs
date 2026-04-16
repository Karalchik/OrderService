using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Services;

namespace OrderService.API.Controllers
{
    /// <summary>
    /// REST API controller for managing customer orders.
    /// Provides endpoints for creating, canceling, and querying orders.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderCommandService _commandService;
        private readonly IOrderQueryService _queryService;

        /// <summary>
        /// Initializes a new instance of <see cref="OrdersController"/>.
        /// </summary>
        /// <param name="commandService">Service handling order creation and cancellation.</param>
        /// <param name="queryService">Service handling order lookups and listing.</param>
        public OrdersController(IOrderCommandService commandService, IOrderQueryService queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }

        /// <summary>Creates a new order.</summary>
        /// <param name="request">Order data containing user name and line items.</param>
        /// <returns>The created order with HTTP 201 and a Location header.</returns>
        /// <response code="201">Order successfully created.</response>
        /// <response code="400">Validation failed (empty user name, invalid items, etc.).</response>
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderRequest request)
        {
            var order = await _commandService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        /// <summary>Cancels an existing order.</summary>
        /// <param name="id">The order identifier.</param>
        /// <returns>The canceled order.</returns>
        /// <response code="200">Order successfully canceled.</response>
        /// <response code="400">Order cannot be canceled (already delivered or canceled).</response>
        /// <response code="404">Order not found.</response>
        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<OrderDto>> Cancel(string id)
        {
            var order = await _commandService.CancelOrderAsync(id);
            return Ok(order);
        }

        /// <summary>Retrieves a single order by its identifier.</summary>
        /// <param name="id">The order identifier.</param>
        /// <returns>The order data.</returns>
        /// <response code="200">Order found.</response>
        /// <response code="404">Order not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(string id)
        {
            var order = await _queryService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        /// <summary>Lists orders with optional filtering and pagination.</summary>
        /// <param name="userName">Filter by user name.</param>
        /// <param name="status">Filter by status: Created, Delivered, or Canceled.</param>
        /// <param name="limit">Maximum number of results (default: 10).</param>
        /// <param name="offset">Number of results to skip (default: 0).</param>
        /// <returns>A list of matching orders.</returns>
        /// <response code="200">Returns the list of orders.</response>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> List(
            [FromQuery] string? userName,
            [FromQuery] string? status,
            [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            var orders = await _queryService.GetOrdersAsync(userName, status, limit, offset);
            return Ok(orders);
        }
    }
}