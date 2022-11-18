using System.ComponentModel.DataAnnotations;

namespace RealtySale.Shared.DTOs;

public class CityDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is mandatory field")]
    [StringLength(30, MinimumLength = 5)]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name must contains only letters")]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Country { get; set; } = string.Empty;
}
