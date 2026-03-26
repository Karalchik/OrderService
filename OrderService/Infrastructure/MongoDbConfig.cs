using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using OrderService.Domain.Models;

namespace OrderService.Infrastructure
{
    public static class MongoDbConfig
    {
        public static void RegisterMappings()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Order)))
            {
                BsonClassMap.RegisterClassMap<Order>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdProperty(c => c.Id)
                      .SetIdGenerator(StringObjectIdGenerator.Instance); //string для Id
                });
            }
        }
    }
}