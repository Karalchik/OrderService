namespace OrderService.Application.DTOs
{
    /// <summary>Data transfer object representing a single line item in an order.</summary>
    public class OrderItemDto
    {
        /// <summary>Name of the product being ordered.</summary>
        public required string ProductName { get; set; }

        /// <summary>Number of units. Must be greater than zero.</summary>
        public int Quantity { get; set; }

        /// <summary>Price per single unit. Must be positive.</summary>
        public decimal Price { get; set; }
    }
}
