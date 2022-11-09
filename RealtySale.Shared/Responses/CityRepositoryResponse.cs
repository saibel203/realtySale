using RealtySale.Shared.Data;

namespace RealtySale.Shared.Responses;

public class CityRepositoryResponse
{
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
    public IEnumerable<object>? CitiesList { get; set; }
    public City? City { get; set; }
}
