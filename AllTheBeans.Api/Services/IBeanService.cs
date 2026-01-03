using AllTheBeans.Api.Models.Dtos;

namespace AllTheBeans.Api.Services;

public interface IBeanService
{
    Task<IEnumerable<BeanDto>> GetAllBeansAsync();
    Task<BeanDto?> GetBeanByIdAsync(int id);
    Task<BeanDto> CreateBeanAsync(CreateBeanDto createDto);
    Task<BeanDto?> UpdateBeanAsync(int id, UpdateBeanDto updateDto);
    Task<bool> DeleteBeanAsync(int id);
    Task<IEnumerable<BeanDto>> SearchBeansAsync(string? name, string? country, string? colour);
}
