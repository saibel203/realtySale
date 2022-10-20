using System.ComponentModel.DataAnnotations;

namespace RealtySale.Api.DTOs;

public class CityDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is mandatory field")]
    [StringLength(30, MinimumLength = 5)]
    [RegularExpression(".*[a-zA-Z]+.*", ErrorMessage = "Only numerics are not allowed")]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Country { get; set; } = string.Empty;
}
