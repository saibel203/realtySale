using RealtySale.Shared.Data;

namespace RealtySale.Shared.Responses;

public class UserRepositoryResponse
{
    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public User? User { get; set; }
    public UserProperty? Favourite { get; set; }
    public bool IsPropertyInFavourite { get; set; }
}
