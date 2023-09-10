using System.Net;

namespace TaskManagementAPI.CustomException;

public class ConflictException : ApplicationException
{
    public ConflictException(string message, HttpStatusCode statusCode) : base(message, statusCode)
    {
    }
}