namespace RealtySale.Shared.DTOs;

public class PropertyListDto
{
    public int Id { get; set; }
    public byte SellRent {get; set;}
    public string Name { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string FurnishingType { get; set; } = string.Empty;
    public int Price { get; set; }
    public int Bhk { get; set; }
    public int BuiltArea { get; set; }
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool ReadyToMove { get; set; }
    public DateTime EstPossessionOn { get; set; }
    public string Photo { get; set; } = string.Empty;
    public DateTime PostedOn { get; set; } = DateTime.Now;
}
