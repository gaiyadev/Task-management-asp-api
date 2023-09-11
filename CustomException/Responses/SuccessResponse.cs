using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Responses;

namespace TaskManagementAPI.CustomException.Responses;

public static class SuccessResponse
{
    public static IActionResult HandleCreated<T>(string message, string? accessToken, T data )
    {
        var apiResponse = new ApiResponse<T>
        {
            Message = message,
            StatusCode = 201,
            Data = data,
            accessToken = accessToken
        };

        return new ObjectResult(apiResponse)
        {
            StatusCode = 201
        };
    }
    
    public static IActionResult HandleOk<T>(string message, T data, string? accessToken)
    {
        var apiResponse = new ApiResponse<T>
        {
            Message = message,
            StatusCode = 200,
            Data = data,
            accessToken = accessToken
        };

        return new ObjectResult(apiResponse)
        {
            StatusCode = 200
        };
    }
}
