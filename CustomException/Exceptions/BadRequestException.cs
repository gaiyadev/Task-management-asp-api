namespace TaskManagementAPI.CustomException.Exceptions;

public class BadRequestException : ApplicationException
{
    protected BadRequestException(string message) : base(message)
    {
    }
}