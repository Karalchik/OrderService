namespace OrderService.Domain.Models
{
    public class OrderItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
