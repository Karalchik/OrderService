using OrderService.Domain.Interfaces;
using OrderService.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using OrderService.Application.Validators;
using MongoDB.Driver;
using OrderService.Application.Services;
using OrderService.Infrastructure.Middleware;
var builder = WebApplication.CreateBuilder(args);

MongoDbConfig.RegisterMappings();

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb")
    ?? "mongodb://localhost:27017";

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
//можно ли это сделать без регестрации?
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderCommandService, OrderCommandService>();
builder.Services.AddScoped<IOrderQueryService, OrderQueryService>();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks()
.AddMongoDb(
    name: "mongodb",
    timeout: TimeSpan.FromSeconds(3)
);
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); 

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapHealthChecks("/health");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1");
    c.RoutePrefix = "swagger";
});

app.MapControllers();

app.Run();
