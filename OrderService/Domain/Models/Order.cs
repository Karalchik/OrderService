using System;
using System.Collections.Generic;

namespace OrderService.Domain.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public const string StatusCreated = "CREATED";
        public const string StatusDelivered = "DELIVERED";
        public const string StatusCanceled = "CANCELED";

        public bool CanBeCanceled()
        {
            return Status != StatusDelivered && Status != StatusCanceled;
        }
    }
}
