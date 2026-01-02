using AllTheBeans.Api.Data;
using AllTheBeans.Api.Models;
using AllTheBeans.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AllTheBeans.Api.Tests.Services;

public class DataImporterTests
{
    private AllTheBeansDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AllTheBeansDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AllTheBeansDbContext(options);
    }

    [Fact]
    public async Task SeedAsync_SeedsData()
    {
        var context = GetContext();
        var mockEnvironment = new Mock<IWebHostEnvironment>();
        var projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".."));
        mockEnvironment.Setup(e => e.ContentRootPath).Returns(Path.Combine(projectRoot, "AllTheBeans.Api"));

        var dataImporter = new DataImporter(context, mockEnvironment.Object);
        await dataImporter.SeedAsync();

        var beans = await context.Beans.ToListAsync();
        Assert.Equal(15, beans.Count);
    }

    [Fact]
    public async Task SeedAsync_DoesNothing_WhenDatabaseAlreadyHasBeans()
    {
        var context = GetContext();
        var mockEnvironment = new Mock<IWebHostEnvironment>();
        var projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".."));
        mockEnvironment.Setup(e => e.ContentRootPath).Returns(Path.Combine(projectRoot, "AllTheBeans.Api"));

        context.Beans.Add(new Bean 
        { 
            Name = "Test Bean", 
            Country = "Test", 
            Colour = "Brown", 
            Cost = 10.00m 
        });
        await context.SaveChangesAsync();

        var dataImporter = new DataImporter(context, mockEnvironment.Object);
        await dataImporter.SeedAsync();

        var beans = await context.Beans.ToListAsync();
        Assert.Single(beans);
    }
}