namespace TaskManagementAPI.CustomException.Exceptions;

public class UnauthorizedException : ApplicationException
{
    protected UnauthorizedException(string message) : base(message)
    {
    }
}