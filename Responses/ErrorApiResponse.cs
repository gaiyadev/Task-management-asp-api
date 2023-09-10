namespace TaskManagementAPI.Responses;

public class ErrorApiResponse
{
    public required string Error { get; set; }
    public required int StatusCode { get; set; }
    public required string Title { get; set; }
}