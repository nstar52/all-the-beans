using System.ComponentModel.DataAnnotations;

namespace AllTheBeans.Api.Models.Dtos;

public class CreateBeanDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Colour { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Cost must be greater than or equal to 0")]
    public decimal Cost { get; set; }

    [MaxLength(500)]
    public string Image { get; set; } = string.Empty;
}
