namespace RealtySale.Shared.Data;

public class BaseDataEntity
{
    public int Id { get; set; }
    public DateTime LastUpdatedOn { get; set; } = DateTime.Now;
    public int LastUpdatedBy { get; set; }
}
