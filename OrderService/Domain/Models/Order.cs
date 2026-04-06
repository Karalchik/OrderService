using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OrderService.Domain.Models
{
    /// <summary>Lifecycle status of an order.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Created,
        Delivered,
        Canceled
    }

    /// <summary>Domain entity representing a customer order.</summary>
    public class Order
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public required OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>Returns <c>true</c> if the order is not already delivered or canceled.</summary>
        public bool CanBeCanceled()
        {
            return Status != OrderStatus.Delivered && Status != OrderStatus.Canceled;
        }
    }
}
