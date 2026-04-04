namespace OrderService.Application.DTOs
{
    public class OrderItemDto
    {
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
