using System.ComponentModel.DataAnnotations;

namespace RealtySale.Api.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Country { get; set; } = string.Empty;
}
