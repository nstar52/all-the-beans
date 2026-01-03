using AllTheBeans.Api.Models.Dtos;

namespace AllTheBeans.Api.Services;

public interface IBeanOfTheDayService
{
    Task<BeanDto?> GetBeanOfTheDayAsync();
    Task<int?> GetTodayBeanIdAsync();
    Task<BeanDto> SelectBeanOfTheDayAsync();
}
