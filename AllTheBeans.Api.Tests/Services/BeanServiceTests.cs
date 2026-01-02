using AllTheBeans.Api.Data;
using AllTheBeans.Api.Models;
using AllTheBeans.Api.Models.Dtos;
using AllTheBeans.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AllTheBeans.Api.Tests.Services;

public class BeanServiceTests
{
    private AllTheBeansDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AllTheBeansDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AllTheBeansDbContext(options);
    }

    private IBeanOfTheDayService GetMockBeanOfTheDayService()
    {
        var mock = new Mock<IBeanOfTheDayService>();
        mock.Setup(s => s.GetTodayBeanIdAsync()).ReturnsAsync((int?)null);
        return mock.Object;
    }

    [Fact]
    public async Task GetAllBeansAsync_ReturnsAllBeans()
    {
        var context = GetContext();
        var mockBOTDService = GetMockBeanOfTheDayService();
        var service = new BeanService(context, mockBOTDService);

        context.Beans.Add(new Bean { Name = "Colombian", Country = "Colombia", Colour = "Brown", Cost = 10.50m });
        context.Beans.Add(new Bean { Name = "Ethiopian", Country = "Ethiopia", Colour = "Dark", Cost = 12.00m });
        await context.SaveChangesAsync();

        var result = await service.GetAllBeansAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetBeanByIdAsync_ReturnsBean()
    {
        var context = GetContext();
        var mockBOTDService = GetMockBeanOfTheDayService();
        var service = new BeanService(context, mockBOTDService);

        var bean = new Bean { Name = "Colombian", Country = "Colombia", Colour = "Brown", Cost = 10.50m };
        context.Beans.Add(bean);
        await context.SaveChangesAsync();

        var result = await service.GetBeanByIdAsync(bean.Id);

        Assert.NotNull(result);
        Assert.Equal("Colombian", result.Name);
    }

    [Fact]
    public async Task GetBeanByIdAsync_ReturnsNullWhenNotFound()
    {
        var context = GetContext();
        var mockBOTDService = GetMockBeanOfTheDayService();
        var service = new BeanService(context, mockBOTDService);

        var result = await service.GetBeanByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateBeanAsync_CreatesBean()
    {
        var context = GetContext();
        var mockBOTDService = GetMockBeanOfTheDayService();
        var service = new BeanService(context, mockBOTDService);

        var createDto = new CreateBeanDto
        {
            Name = "Kenyan",
            Description = "A nice coffee",
            Country = "Kenya",
            Colour = "Brown",
            Cost = 15.00m,
            Image = "kenyan.jpg"
        };

        var result = await service.CreateBeanAsync(createDto);

        Assert.Equal("Kenyan", result.Name);
        Assert.True(result.Id > 0);

        var saved = await context.Beans.FindAsync(result.Id);
        Assert.NotNull(saved);
        Assert.Equal("Kenyan", saved.Name);
    }

    [Fact]
    public async Task UpdateBeanAsync_UpdatesBean()
    {
        var context = GetContext();
        var mockBOTDService = GetMockBeanOfTheDayService();
        var service = new BeanService(context, mockBOTDService);

        var bean = new Bean { Name = "Original", Country = "Colombia", Colour = "Brown", Cost = 10.50m };
        context.Beans.Add(bean);
        await context.SaveChangesAsync();

        var updateDto = new UpdateBeanDto { Name = "Updated" };
        var result = await service.UpdateBeanAsync(bean.Id, updateDto);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Name);
        
        var updated = await context.Beans.FindAsync(bean.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated", updated.Name);
    }

    [Fact]
    public async Task UpdateBeanAsync_ReturnsNullWhenNotFound()
    {
        var context = GetContext();
        var mockBOTDService = GetMockBeanOfTheDayService();
        var service = new BeanService(context, mockBOTDService);

        var result = await service.UpdateBeanAsync(999, new UpdateBeanDto { Name = "Updated Name" });

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteBeanAsync_DeletesBean()
    {
        var context = GetContext();
        var mockBOTDService = GetMockBeanOfTheDayService();
        var service = new BeanService(context, mockBOTDService);

        var bean = new Bean { Name = "Colombian", Country = "Colombia", Colour = "Brown", Cost = 10.50m };
        context.Beans.Add(bean);
        await context.SaveChangesAsync();

        var result = await service.DeleteBeanAsync(bean.Id);

        Assert.True(result);
        Assert.Null(await context.Beans.FindAsync(bean.Id));
    }

    [Fact]
    public async Task DeleteBeanAsync_ReturnsFalseWhenNotFound()
    {
        var context = GetContext();
        var mockBOTDService = GetMockBeanOfTheDayService();
        var service = new BeanService(context, mockBOTDService);

        var result = await service.DeleteBeanAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task SearchBeansAsync_FiltersByName()
    {
        var context = GetContext();
        var mockBOTDService = GetMockBeanOfTheDayService();
        var service = new BeanService(context, mockBOTDService);

        context.Beans.Add(new Bean { Name = "Colombian", Country = "Colombia", Colour = "Brown", Cost = 10.50m });
        context.Beans.Add(new Bean { Name = "Ethiopian", Country = "Ethiopia", Colour = "Brown", Cost = 12.00m });
        await context.SaveChangesAsync();

        var result = await service.SearchBeansAsync("Colombian", null, null);

        Assert.Single(result);
        Assert.Equal("Colombian", result.First().Name);
    }

}