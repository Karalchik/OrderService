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

builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration));

MongoDbConfig.RegisterMappings();

var connectionStringOptions = builder.Configuration
    .GetSection("ConnectionStrings")
    .Get<ConnectionStringOptions>() ?? new ConnectionStringOptions();

builder.Services.Configure<ConnectionStringOptions>(
    builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionStringOptions.MongoDb));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = connectionStringOptions.Redis;
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
    .AddRedis(connectionStringOptions.Redis, name: "redis");

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
