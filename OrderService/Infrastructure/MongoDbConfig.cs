using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using OrderService.Domain.Models;

namespace OrderService.Infrastructure
{
    public static class MongoDbConfig
    {
        public static void RegisterMappings()
        {
            BsonSerializer.TryRegisterSerializer(new EnumSerializer<OrderStatus>(BsonType.String));

            if (!BsonClassMap.IsClassMapRegistered(typeof(Order)))
            {
                BsonClassMap.RegisterClassMap<Order>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdProperty(c => c.Id)
                      .SetIdGenerator(StringObjectIdGenerator.Instance);
                });
            }
        }
    }
}