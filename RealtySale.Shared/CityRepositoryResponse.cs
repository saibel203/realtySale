namespace RealtySale.Shared;

public class CityRepositoryResponse
{
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
    public IEnumerable<object>? CitiesList { get; set; }
    public object? City { get; set; }
}
