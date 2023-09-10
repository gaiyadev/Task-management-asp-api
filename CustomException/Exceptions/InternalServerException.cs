
namespace TaskManagementAPI.CustomException.Exceptions;

public class InternalServerException : ApplicationException
{
    public InternalServerException(string message) : base(message)
    {
    }
}