using Microsoft.AspNetCore.Mvc;
using AllTheBeans.Api.Models.Dtos;
using AllTheBeans.Api.Services;

namespace AllTheBeans.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BeansController : ControllerBase
{
    private readonly IBeanService _beanService;

    public BeansController(IBeanService beanService)
    {
        _beanService = beanService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BeanDto>>> GetAllBeans()
    {
        var beans = await _beanService.GetAllBeansAsync();
        return Ok(beans);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BeanDto>> GetBeanById(int id)
    {
        var bean = await _beanService.GetBeanByIdAsync(id);
        if (bean == null)
        {
            return NotFound();
        }
        return Ok(bean);
    }

    [HttpPost]
    public async Task<ActionResult<BeanDto>> CreateBean([FromBody] CreateBeanDto createDto)
    {
        var bean = await _beanService.CreateBeanAsync(createDto);
        return CreatedAtAction(nameof(GetBeanById), new { id = bean.Id }, bean);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BeanDto>> UpdateBean(int id, [FromBody] UpdateBeanDto updateDto)
    {
        var bean = await _beanService.UpdateBeanAsync(id, updateDto);
        if (bean == null)
        {
            return NotFound();
        }
        return Ok(bean);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBean(int id)
    {
        var deleted = await _beanService.DeleteBeanAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<BeanDto>>> SearchBeans(
        [FromQuery] string? name,
        [FromQuery] string? country,
        [FromQuery] string? colour)
    {
        var beans = await _beanService.SearchBeansAsync(name, country, colour);
        return Ok(beans);
    }
}

