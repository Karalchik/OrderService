namespace OrderService.Domain.Models
{
    /// <summary>Single line item within an <see cref="Order"/>.</summary>
    public class OrderItem
    {
        /// <summary>Name of the product being ordered.</summary>
        public required string ProductName { get; set; }

        /// <summary>Number of units ordered. Must be greater than zero.</summary>
        public int Quantity { get; set; }

        /// <summary>Price per single unit of the product.</summary>
        public decimal Price { get; set; }
    }
}
