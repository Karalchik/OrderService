using System.Text.Json.Serialization;

namespace OrderService.Domain.Models
{
    /// <summary>Lifecycle status of an order.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        /// <summary>Order has been created and is awaiting processing.</summary>
        Created,

        /// <summary>Order has been delivered to the customer.</summary>
        Delivered,

        /// <summary>Order has been canceled by the user or system.</summary>
        Canceled
    }
}
