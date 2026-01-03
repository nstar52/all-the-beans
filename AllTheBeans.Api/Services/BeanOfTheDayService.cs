using AllTheBeans.Api.Data;
using AllTheBeans.Api.Models;
using AllTheBeans.Api.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AllTheBeans.Api.Services;

public class BeanOfTheDayService : IBeanOfTheDayService
{
    private readonly AllTheBeansDbContext _context;

    public BeanOfTheDayService(AllTheBeansDbContext context)
    {
        _context = context;
    }

    public async Task<BeanDto?> GetBeanOfTheDayAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var beanOfTheDay = await _context.BeanOfTheDays
            .Include(b => b.Bean)
            .FirstOrDefaultAsync(b => b.Date == today);

        if (beanOfTheDay == null)
        {
            return await SelectBeanOfTheDayAsync();
        }

        return BeanMapper.ToDto(beanOfTheDay.Bean, true);
    }

    public async Task<int?> GetTodayBeanIdAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var beanOfTheDay = await _context.BeanOfTheDays
            .FirstOrDefaultAsync(b => b.Date == today);

        return beanOfTheDay?.BeanId;
    }

    public async Task<BeanDto> SelectBeanOfTheDayAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var existingBOTD = await _context.BeanOfTheDays
            .FirstOrDefaultAsync(b => b.Date == today);

        if (existingBOTD != null)
        {
            var bean = await _context.Beans.FindAsync(existingBOTD.BeanId);
            if (bean != null)
            {
                return BeanMapper.ToDto(bean, true);
            }
        }

        var yesterday = today.AddDays(-1);
        var yesterdayBean = await _context.BeanOfTheDays
            .Include(b => b.Bean)
            .FirstOrDefaultAsync(b => b.Date == yesterday);

        // Make sure we don't pick the same bean as yesterday
        var availableBeans = await _context.Beans.ToListAsync();
        var beansToChooseFrom = yesterdayBean != null
            ? availableBeans.Where(b => b.Id != yesterdayBean.BeanId).ToList()
            : availableBeans;

        if (beansToChooseFrom.Count == 0)
        {
            throw new InvalidOperationException("No beans available to select");
        }

        var random = new Random();
        var selectedBean = beansToChooseFrom[random.Next(beansToChooseFrom.Count)];

        if (yesterdayBean != null && yesterdayBean.Bean != null)
        {
            yesterdayBean.Bean.IsBeanOfTheDay = false;
        }

        selectedBean.IsBeanOfTheDay = true;

        var beanOfTheDay = new BeanOfTheDay
        {
            BeanId = selectedBean.Id,
            Date = today
        };
        _context.BeanOfTheDays.Add(beanOfTheDay);
        await _context.SaveChangesAsync();

        return BeanMapper.ToDto(selectedBean, true);
    }
}

