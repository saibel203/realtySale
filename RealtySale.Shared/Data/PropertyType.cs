using System.ComponentModel.DataAnnotations;

namespace RealtySale.Shared.Data;

public class PropertyType : BaseDataEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
