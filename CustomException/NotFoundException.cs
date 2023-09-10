using System.Net;

namespace TaskManagementAPI.CustomException;

public class NotFoundException : ApplicationException
{
    protected NotFoundException(string message, HttpStatusCode statusCode) : base(message, statusCode)
    {
    }
}