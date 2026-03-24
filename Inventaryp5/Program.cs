using Microsoft.EntityFrameworkCore;
using PharmacyOrderingSystemp5.Data;

// PERSON-5 FILES
using PharmacyOrderingSystemp5.Repositories.Interfaces;
using PharmacyOrderingSystemp5.Repositories.Implementations;
using PharmacyOrderingSystemp5.Services.Interfaces;
using PharmacyOrderingSystemp5.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Database Connection (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Dependency Injection (Person-5)
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// CORS (required for Angular front-end later)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// CORS middleware
app.UseCors("AllowAll");

app.UseHttpsRedirection();

// NO AUTH NOW
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();