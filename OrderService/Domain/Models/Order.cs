using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OrderService.Domain.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Created,
        Delivered,
        Canceled
    }
    public class Order
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public required OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool CanBeCanceled()
        {
            return Status != OrderStatus.Delivered && Status != OrderStatus.Canceled;
        }
    }
}
