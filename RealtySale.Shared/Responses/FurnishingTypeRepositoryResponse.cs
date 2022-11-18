using RealtySale.Shared.Data;

namespace RealtySale.Shared.Responses;

public class FurnishingTypeRepositoryResponse
{
    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public IEnumerable<FurnishingType>? FurnishingTypes { get; set; }
}
