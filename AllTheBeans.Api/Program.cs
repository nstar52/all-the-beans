using Microsoft.EntityFrameworkCore;
using AllTheBeans.Api.Data;
using AllTheBeans.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Security: Configure HSTS for production
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

var dbPath = Path.Combine(builder.Environment.ContentRootPath, "allthebeans.db");
builder.Services.AddDbContext<AllTheBeansDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped<IBeanService, BeanService>();
builder.Services.AddScoped<IBeanOfTheDayService, BeanOfTheDayService>();
builder.Services.AddScoped<DataImporter>();

var app = builder.Build();

// Apply migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AllTheBeansDbContext>();
    await context.Database.MigrateAsync();
    
    var dataImporter = scope.ServiceProvider.GetRequiredService<DataImporter>();
    await dataImporter.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
