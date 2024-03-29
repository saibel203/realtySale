﻿namespace RealtySale.Shared.DTOs;

public class PropertyDetailDto : PropertyListDto
{
    public int CarpetArea { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public int FloorNo { get; set; }
    public int TotalFloors { get; set; }
    public string MainEntrance { get; set; } = string.Empty;
    public int Security { get; set; }
    public bool Gated { get; set; }
    public int Maintenance { get; set; }
    public int Age { get; set; }
    public ICollection<PhotoDto?>? Photos { get; set; }
}
    