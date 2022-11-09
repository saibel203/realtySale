using System.ComponentModel.DataAnnotations;

namespace RealtySale.Shared.Data;

public class City : BaseDataEntity
{
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Country { get; set; } = string.Empty;
}
