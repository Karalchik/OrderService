namespace OrderService.Application.DTOs
{
    public class CreateOrderRequest
    {
        public string UserId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
