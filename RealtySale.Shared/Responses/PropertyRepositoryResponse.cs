using RealtySale.Shared.Data;

namespace RealtySale.Shared.Responses;

public class PropertyRepositoryResponse
{
    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public IEnumerable<Property>? Properties { get; set; } = new List<Property>();
    public Property? Property { get; set; } = new();
}
