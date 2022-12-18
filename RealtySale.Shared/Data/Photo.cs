using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtySale.Shared.Data;

[Table("Photos")]
public class Photo : BaseDataEntity
{
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public int PropertyId { get; set; }
    public Property? Property { get; set; }
}
