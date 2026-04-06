namespace OrderService.Domain.Models
{
    /// <summary>Single line item within an <see cref="Order"/>.</summary>
    public class OrderItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
