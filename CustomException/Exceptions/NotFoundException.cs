namespace TaskManagementAPI.CustomException.Exceptions;

public class NotFoundException : ApplicationException
{
    protected internal NotFoundException(string message) : base(message)
    {
    }
}