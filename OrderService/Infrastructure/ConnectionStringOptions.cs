namespace OrderService.Infrastructure;

public class ConnectionStringOptions
{
    public string MongoDb { get; set; } = "mongodb://localhost:27017";
    public string Redis { get; set; } = "localhost:6379";
}
