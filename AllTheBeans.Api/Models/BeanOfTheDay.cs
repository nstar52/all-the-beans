namespace AllTheBeans.Api.Models;

public class BeanOfTheDay
{
    public int Id { get; set; }
    public int BeanId { get; set; }
    public DateOnly Date { get; set; }
    public Bean Bean { get; set; } = null!;
}

