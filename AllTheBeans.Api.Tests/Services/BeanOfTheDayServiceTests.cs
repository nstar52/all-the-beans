using AllTheBeans.Api.Data;
using AllTheBeans.Api.Models;
using AllTheBeans.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AllTheBeans.Api.Tests.Services;

public class BeanOfTheDayServiceTests
{
    private AllTheBeansDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AllTheBeansDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AllTheBeansDbContext(options);
    }

    [Fact]
    public async Task SelectBeanOfTheDayAsync_SelectsRandomBean()
    {
        var context = GetContext();
        var service = new BeanOfTheDayService(context);

        context.Beans.Add(new Bean { Name = "Colombian", Country = "Colombia", Colour = "Brown", Cost = 10.50m });
        context.Beans.Add(new Bean { Name = "Ethiopian", Country = "Ethiopia", Colour = "Brown", Cost = 12.00m });
        context.Beans.Add(new Bean { Name = "Brazilian", Country = "Brazil", Colour = "Brown", Cost = 9.50m });
        await context.SaveChangesAsync();

        var selected = await service.SelectBeanOfTheDayAsync();

        Assert.NotNull(selected);
        Assert.True(selected.IsBeanOfTheDay);
    }

    [Fact]
    public async Task SelectBeanOfTheDayAsync_CannotBeSameAsPreviousDay()
    {
        var context = GetContext();
        var service = new BeanOfTheDayService(context);

        var bean1 = new Bean { Name = "Colombian", Country = "Colombia", Colour = "Brown", Cost = 10.50m };
        var bean2 = new Bean { Name = "Ethiopian", Country = "Ethiopia", Colour = "Brown", Cost = 12.00m };
        var bean3 = new Bean { Name = "Brazilian", Country = "Brazil", Colour = "Brown", Cost = 9.50m };
        
        context.Beans.AddRange(new[] { bean1, bean2, bean3 });
        await context.SaveChangesAsync();

        var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        context.BeanOfTheDays.Add(new BeanOfTheDay { BeanId = bean1.Id, Date = yesterday });
        await context.SaveChangesAsync();

        var selected = await service.SelectBeanOfTheDayAsync();

        Assert.NotNull(selected);
        Assert.NotEqual(bean1.Id, selected.Id);
    }

    [Fact]
    public async Task SelectBeanOfTheDayAsync_ThrowsWhenNoBeansAvailable()
    {
        var context = GetContext();
        var service = new BeanOfTheDayService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.SelectBeanOfTheDayAsync());
    }

    [Fact]
    public async Task SelectBeanOfTheDayAsync_WorksWithOnlyOneBean()
    {
        var context = GetContext();
        var service = new BeanOfTheDayService(context);

        var bean = new Bean { Name = "Colombian", Country = "Colombia", Colour = "Brown", Cost = 10.50m };
        context.Beans.Add(bean);
        await context.SaveChangesAsync();

        var selected = await service.SelectBeanOfTheDayAsync();

        Assert.NotNull(selected);
        Assert.Equal(bean.Id, selected.Id);
    }

    [Fact]
    public async Task GetBeanOfTheDayAsync_ReturnsExistingIfAlreadySelected()
    {
        var context = GetContext();
        var service = new BeanOfTheDayService(context);

        var bean = new Bean { Name = "Colombian", Country = "Colombia", Colour = "Brown", Cost = 10.50m };
        context.Beans.Add(bean);
        await context.SaveChangesAsync();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        context.BeanOfTheDays.Add(new BeanOfTheDay { BeanId = bean.Id, Date = today });
        await context.SaveChangesAsync();

        var result = await service.GetBeanOfTheDayAsync();

        Assert.NotNull(result);
        Assert.Equal(bean.Id, result.Id);
        Assert.True(result.IsBeanOfTheDay);
    }

}

