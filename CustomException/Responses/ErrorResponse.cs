namespace TaskManagementAPI.CustomException.Responses;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public required string Message { get; set; }
    public required string ExceptionMessage { get; set; }
    public string? StackTrace { get; set; }
}