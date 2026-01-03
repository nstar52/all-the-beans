using AllTheBeans.Api.Models;
using AllTheBeans.Api.Models.Dtos;

namespace AllTheBeans.Api.Services;

public static class BeanMapper
{
    public static BeanDto ToDto(Bean bean, bool isBeanOfTheDay)
    {
        return new BeanDto
        {
            Id = bean.Id,
            Name = bean.Name,
            Description = bean.Description,
            Country = bean.Country,
            Colour = bean.Colour,
            Cost = bean.Cost,
            Image = bean.Image,
            IsBeanOfTheDay = isBeanOfTheDay
        };
    }
}

