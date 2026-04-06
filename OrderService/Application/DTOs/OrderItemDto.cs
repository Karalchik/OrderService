namespace OrderService.Application.DTOs
{
    /// <summary>DTO for a single order line item.</summary>
    public class OrderItemDto
    {
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
