using Microsoft.AspNetCore.Mvc;
using AllTheBeans.Api.Models.Dtos;
using AllTheBeans.Api.Services;

namespace AllTheBeans.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BeanOfTheDayController : ControllerBase
{
    private readonly IBeanOfTheDayService _beanOfTheDayService;

    public BeanOfTheDayController(IBeanOfTheDayService beanOfTheDayService)
    {
        _beanOfTheDayService = beanOfTheDayService;
    }

    [HttpGet]
    public async Task<ActionResult<BeanDto>> GetBeanOfTheDay()
    {
        var bean = await _beanOfTheDayService.GetBeanOfTheDayAsync();
        if (bean == null)
        {
            return NotFound();
        }
        return Ok(bean);
    }
}

