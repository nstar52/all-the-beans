using System.Text.Json;
using AllTheBeans.Api.Data;
using AllTheBeans.Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace AllTheBeans.Api.Services;

public class DataImporter
{
    private readonly AllTheBeansDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public DataImporter(AllTheBeansDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task SeedAsync()
    {
        if (await _context.Beans.AnyAsync())
        {
            return;
        }

        var jsonPath = Path.Combine(_environment.ContentRootPath, "..", "AllTheBeans.json");
        if (!File.Exists(jsonPath))
            return;

        var jsonContent = await File.ReadAllTextAsync(jsonPath);
        var jsonBeans = JsonSerializer.Deserialize<List<JsonBean>>(jsonContent);

        if (jsonBeans == null || jsonBeans.Count == 0)
        {
            return;
        }

        var beans = new List<Bean>();

        foreach (var jsonBean in jsonBeans)
        {
            var cost = ParseCost(jsonBean.Cost);
            var bean = new Bean
            {
                ExternalId = jsonBean._id,
                Index = jsonBean.index,
                Name = jsonBean.Name,
                Description = jsonBean.Description.Trim(),
                Country = jsonBean.Country,
                Colour = jsonBean.colour,
                Cost = cost,
                Image = jsonBean.Image,
                IsBeanOfTheDay = jsonBean.isBOTD
            };
            beans.Add(bean);
        }

        await _context.Beans.AddRangeAsync(beans);
        await _context.SaveChangesAsync();

        // If there's a bean marked as BOTD in the JSON, create the record for today
        // Need to save beans first so we have the IDs
        var initialBOTD = beans.FirstOrDefault(b => b.IsBeanOfTheDay);
        if (initialBOTD != null)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var beanOfTheDay = new BeanOfTheDay
            {
                BeanId = initialBOTD.Id,
                Date = today
            };
            await _context.BeanOfTheDays.AddAsync(beanOfTheDay);
            await _context.SaveChangesAsync();
        }
    }

    private decimal ParseCost(string costString)
    {
        if (string.IsNullOrWhiteSpace(costString))
            return 0;

        var cleaned = costString.Replace("£", "").Trim();
        if (decimal.TryParse(cleaned, out var cost))
            return cost;

        return 0;
    }

    private class JsonBean
    {
        public string _id { get; set; } = string.Empty;
        public int index { get; set; }
        public bool isBOTD { get; set; }
        public string Cost { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string colour { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}

