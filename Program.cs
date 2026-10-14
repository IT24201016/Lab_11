using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// 1. Production CORS Policy allowing Vercel frontend origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(
            "https://it24100131.github.io",
            "https://campuseats-web.vercel.app",
            "http://localhost:5173"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// 2. Read database connection string from environment variable (ConnectionStrings__Default)
var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in development or for quick test verification
app.UseSwagger();
app.UseSwaggerUI();

// Apply CORS policy before MapControllers()
app.UseCors("Frontend");

app.UseAuthorization();

// Public Health Check & Test Endpoint
app.MapGet("/", () => Results.Ok(new { status = "Online", service = "CampusEats API", tier = "Backend REST API" }));
app.MapGet("/api/menu", () => Results.Ok(new[]
{
    new { id = 1, name = "Campus Chicken Rice & Curry", price = "Rs. 450.00", category = "Lunch", available = true },
    new { id = 2, name = "Crispy Veggie Sub Burger", price = "Rs. 380.00", category = "Fast Food", available = true },
    new { id = 3, name = "Fresh Tropical Fruit Salad", price = "Rs. 250.00", category = "Dessert", available = true },
    new { id = 4, name = "Iced Milk Coffee", price = "Rs. 200.00", category = "Beverage", available = true }
}));

app.MapControllers();

app.Run();
