namespace RealtySale.Shared.DTOs;

public class PropertyDto
{
    public int SellRent { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PropertyTypeId { get; set; }
    public int FurnishingTypeId { get; set; }
    public int Price { get; set; }
    public int Bhk { get; set; }
    public int BuiltArea { get; set; }
    public int CityId { get; set; }
    public bool ReadyToMove { get; set; }
    public int CarpetArea { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public int FloorNo { get; set; }
    public int TotalFloors { get; set; }
    public string MainEntrance { get; set; } = string.Empty;
    public int Security { get; set; }
    public bool Gated { get; set; }
    public int Maintenance { get; set; }
    public DateTime EstPossessionOn { get; set; }
    public int Age { get; set; }
    public string Description { get; set; }  = string.Empty;
    public DateTime PostedOn { get; set; } = DateTime.Now;
}
