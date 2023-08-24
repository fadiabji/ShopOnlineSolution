using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnlie.Api.Data;
using ShopOnline.Api.Repositories.Contracts;

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
builder.Services.AddScoped<IShopingCartReopository, ShopingCartReopository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// when I start the web app the first time I get the Error tells that 
// That because the Blazor component has differnet url from api.web component
//Failed to load resource: the server responded with a status of 404 ()
//Access to fetch at 'https://localhost:7106/api/Product' from origin 'https://localhost:7166' has been blocked by CORS policy:
//No 'Access-Control-Allow-Origin' header is present on the requested resource. If an opaque response serves your needs,
//set the request's mode to 'no-cors' to fetch the resource with CORS disabled.
app.UseCors(police => police.WithOrigins("https://localhost:7166", "https://localhost:7166")
                                .AllowAnyMethod()
                                .WithHeaders(HeaderNames.ContentType)

    );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
