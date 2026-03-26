using OrderService.Domain.Interfaces;
using OrderService.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using OrderService.Application.Validators;
var builder = WebApplication.CreateBuilder(args);

OrderService.Infrastructure.MongoDbConfig.RegisterMappings();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<OrderService.Application.Services.OrderCommandService>();
builder.Services.AddScoped<OrderService.Application.Services.OrderQueryService>();
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

app.UseMiddleware<OrderService.Infrastructure.Middleware.ExceptionHandlingMiddleware>();

app.MapHealthChecks("/health");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1");
    c.RoutePrefix = "swagger";
});

app.MapControllers();

app.Run();
