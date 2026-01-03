using AllTheBeans.Api.Data;
using AllTheBeans.Api.Models;
using AllTheBeans.Api.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AllTheBeans.Api.Services;

public class BeanService : IBeanService
{
    private readonly AllTheBeansDbContext _context;
    private readonly IBeanOfTheDayService _beanOfTheDayService;

    public BeanService(AllTheBeansDbContext context, IBeanOfTheDayService beanOfTheDayService)
    {
        _context = context;
        _beanOfTheDayService = beanOfTheDayService;
    }

    public async Task<IEnumerable<BeanDto>> GetAllBeansAsync()
    {
        var todayBeanId = await _beanOfTheDayService.GetTodayBeanIdAsync();
        var beans = await _context.Beans.ToListAsync();
        return beans.Select(b => BeanMapper.ToDto(b, b.Id == todayBeanId));
    }

    public async Task<BeanDto?> GetBeanByIdAsync(int id)
    {
        var bean = await _context.Beans.FindAsync(id);
        if (bean == null)
            return null;

        var todayBeanId = await _beanOfTheDayService.GetTodayBeanIdAsync();
        var isBOTD = bean.Id == todayBeanId;
        return BeanMapper.ToDto(bean, isBOTD);
    }

    public async Task<BeanDto> CreateBeanAsync(CreateBeanDto createDto)
    {
        var bean = new Bean
        {
            Name = createDto.Name,
            Description = createDto.Description,
            Country = createDto.Country,
            Colour = createDto.Colour,
            Cost = createDto.Cost,
            Image = createDto.Image,
            ExternalId = string.Empty,
            Index = 0,
            IsBeanOfTheDay = false
        };

        _context.Beans.Add(bean);
        await _context.SaveChangesAsync();

        return BeanMapper.ToDto(bean, isBeanOfTheDay: false);
    }

    public async Task<BeanDto?> UpdateBeanAsync(int id, UpdateBeanDto updateDto)
    {
        var bean = await _context.Beans.FindAsync(id);
        if (bean == null)
            return null;

        if (updateDto.Name != null) bean.Name = updateDto.Name;
        if (updateDto.Description != null) bean.Description = updateDto.Description;
        if (updateDto.Country != null) bean.Country = updateDto.Country;
        if (updateDto.Colour != null) bean.Colour = updateDto.Colour;
        if (updateDto.Cost.HasValue) bean.Cost = updateDto.Cost.Value;
        if (updateDto.Image != null) bean.Image = updateDto.Image;

        await _context.SaveChangesAsync();

        var todayBeanId = await _beanOfTheDayService.GetTodayBeanIdAsync();
        return BeanMapper.ToDto(bean, bean.Id == todayBeanId);
    }

    public async Task<bool> DeleteBeanAsync(int id)
    {
        var bean = await _context.Beans.FindAsync(id);
        if (bean == null)
            return false;

        _context.Beans.Remove(bean);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<BeanDto>> SearchBeansAsync(string? name, string? country, string? colour)
    {
        var query = _context.Beans.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(b => b.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(country))
        {
            query = query.Where(b => b.Country == country);
        }

        if (!string.IsNullOrWhiteSpace(colour))
        {
            query = query.Where(b => b.Colour == colour);
        }

        var beans = await query.ToListAsync();
        var todayBeanId = await _beanOfTheDayService.GetTodayBeanIdAsync();

        return beans.Select(b => BeanMapper.ToDto(b, b.Id == todayBeanId));
    }
}

