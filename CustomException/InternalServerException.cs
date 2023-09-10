using System.Net;

namespace TaskManagementAPI.CustomException;

public class InternalServerException : ApplicationException
{
    public InternalServerException(string message, HttpStatusCode statusCode) : base(message, statusCode)
    {
    }
}