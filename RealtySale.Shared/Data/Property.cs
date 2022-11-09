﻿using System.ComponentModel.DataAnnotations.Schema;

namespace RealtySale.Shared.Data;

public class Property : BaseDataEntity
{
    public byte SellRent { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PropertyTypeId { get; set; }
    public PropertyType? PropertyType { get; set; }
    public int Bhk { get; set; }
    public int FurnishingTypeId { get; set; }
    public FurnishingType? FurnishingType { get; set; }
    public int Price { get; set; }
    public int BuiltArea { get; set; }
    public int CarpetArea { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public int CityId { get; set; }
    public City? City { get; set; }
    public int FloorNo { get; set; }
    public int TotalFloors { get; set; }
    public bool ReadyToMove { get; set; }
    public string MainEntrance { get; set; } = string.Empty;
    public int Security { get; set; }
    public bool Gated { get; set; }
    public int Maintenance { get; set; }
    public DateTime EstPossessionOn { get; set; }
    public int Age { get; set; }
    public string Description { get; set; } = string.Empty;
    public ICollection<Photo>? Photos { get; set; }
    public DateTime PostedOn { get; set; } = DateTime.Now;
    [ForeignKey("User")]
    public int PostedBy { get; set; }
    public User? User { get; set; }
}
