namespace RealtySale.Shared.Responses;

public class PhotoServiceResponse
{
    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string ImagePath { get; set; } = string.Empty;
}
