namespace AllTheBeans.Api.Models.Dtos;

public class CreateBeanDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Colour { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string Image { get; set; } = string.Empty;
}

