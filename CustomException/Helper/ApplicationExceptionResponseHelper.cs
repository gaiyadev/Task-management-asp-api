using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Responses;

namespace TaskManagementAPI.CustomException.Helper
{
    public static class ApplicationExceptionResponseHelper
    {
        public static IActionResult HandleNotFound(string errorMessage)
        {
            var notFoundApiResponse = new ErrorApiResponse
            {
                Error = errorMessage,
                StatusCode = 404,
                Title =  HttpStatusTitles.NotFound,
            };
            return new ObjectResult(notFoundApiResponse)
            {
                StatusCode = 404
            };
        }

        public static IActionResult HandleForbidden(string errorMessage)
        {
            var forbidApiResponse = new ErrorApiResponse
            {
                Error = errorMessage,
                StatusCode = 403,
                Title =  HttpStatusTitles.Forbidden,
            };
            return new ObjectResult(forbidApiResponse)
            {
                StatusCode = 403
            };
        }

        public static IActionResult HandleBadRequest(string errorMessage)
        {
            var badRequestApiResponse = new ErrorApiResponse
            {
                Error = errorMessage,
                StatusCode = 400,
                Title = HttpStatusTitles.BadRequest,
            };
            return new ObjectResult(badRequestApiResponse)
            {
                StatusCode = 400
            };
        }

  
        
        public static IActionResult HandleConflictException(string errorMessage)
        {
            var conflictResponse = new ErrorApiResponse
            {
                Error = errorMessage,
                StatusCode = 409,
                Title =  HttpStatusTitles.Conflict,
            };
            return new ObjectResult(conflictResponse)
            {
                StatusCode = 409
            };
        }
        
        
        public static IActionResult HandleInternalServerError(string errorMessage)
        {
            var internalServerErrorApiResponse = new ErrorApiResponse
            {
                Error = errorMessage,
                StatusCode = 500,
                Title =  HttpStatusTitles.InternalServerError,
            };
            return new ObjectResult(internalServerErrorApiResponse)
            {
                StatusCode = 500
            };
        }
    }
}
