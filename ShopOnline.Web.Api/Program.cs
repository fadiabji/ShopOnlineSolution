using Microsoft.EntityFrameworkCore;
using ShopOnlie.Api.Data;
using ShopOnline.Web.Server.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add connectionString
var connectionString = builder.Configuration.GetConnectionString("ShopOnlineConnection");
builder.Services.AddDbContext<ShopOnlineDbContext>(options =>
    options.UseSqlServer(connectionString));

// Dependency injection
builder.Services.AddScoped<IProductRepostiories, ProductRepostiories>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
