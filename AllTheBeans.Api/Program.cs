using Microsoft.EntityFrameworkCore;
using AllTheBeans.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var dbPath = Path.Combine(builder.Environment.ContentRootPath, "allthebeans.db");
builder.Services.AddDbContext<AllTheBeansDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
