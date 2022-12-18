using RealtySale.Shared.Data;

namespace RealtySale.Shared.Responses;

public class PropertyTypeRepositoryResponse
{
    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public IEnumerable<PropertyType>? PropertyTypes { get; set; }
}
