using FluentValidation;
using FluentValidation.AspNetCore;
using MongoDB.Driver;
using OrderService.Application.Mapping;
using OrderService.Application.Services;
using OrderService.Application.Validators;
using OrderService.Domain.Interfaces;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog: reads "Serilog" section from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

MongoDbConfig.RegisterMappings();
//options
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb") ?? "mongodb://localhost:27017";
var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "OrderService_";
});
// Application services
builder.Services.AddSingleton<OrderMapper>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderCommandService, OrderCommandService>();
builder.Services.AddScoped<IOrderQueryService, OrderQueryService>();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

builder.Services.AddHealthChecks()
    .AddMongoDb(name: "mongodb")
    .AddRedis(redisConnectionString, name: "redis");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapHealthChecks("/health");
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1");
    c.RoutePrefix = "swagger";
});
app.MapControllers();
app.Run();
