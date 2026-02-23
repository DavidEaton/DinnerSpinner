using System;
using System.Collections.Generic;
using DinnerSpinner.Domain.Shared;
using Microsoft.AspNetCore.Http;
using IResult = Microsoft.AspNetCore.Http.IResult;
using Result = DinnerSpinner.Domain.Shared.Result;

namespace DinnerSpinner.Api.Common;

public static class ResultExtensions
{
    public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<Error, T> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }

    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        // Problem Details for HTTP APIs (https://www.rfc-editor.org/rfc/rfc9457.html) 
        // The ProblemDetails Class is a standard way to return error details in HTTP APIs, as defined in RFC 7807. It provides a consistent format for error responses, making it easier for clients to understand and handle errors. The ProblemDetails class includes properties such as Type, Title, Status, Detail, and Instance, which can be used to convey information about the error that occurred. Here, we are using the ProblemDetails class to return error information based on the type of error that occurred in the Result object. The GetStatusCode, GetTitle, and GetType methods are used to determine the appropriate HTTP status code, title, and type for the error response based on the ErrorType of the error. For example, if the error is a validation error, we return a 400 Bad Request status code, a title of "Bad Request", and a type that points to the relevant section of the RFC that defines the error. This allows clients to easily understand the nature of the error and how to handle it appropriately. Here is an example response from the ToProblemDetails extension method in an API endpoint: 
        // HTTP/1.1 400 Bad Request
        // Content-Type: application/problem+json
        // {
        //   "type": "https://tools.ietf.org/html/rfc7231#section—6.5.1",
        //   "title": "Bad Request",
        //   "status": 400,
        //   "detail": "Validation error occurred",
        //   "instance": "/api/endpoint",
        //   "errors": [
        //     {
        //       "code": "ValidationError",
        //       "description": "The provided input is invalid.",
        //       "type": "Validation"
        //     }
        //   ]
        // }

        return Results.Problem(
            type: GetType(result.Error.Type),
            statusCode: GetStatusCode(result.Error.Type),
            title: GetTitle(result.Error.Type),
            detail: result.Error.Description,
            instance: result.Error.Code,
            extensions: new Dictionary<string, object?>
            {
                { "errors", new[] { result.Error } }
            });

        static int GetStatusCode(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

        static string GetTitle(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.NotFound => "Not Found",
                ErrorType.Conflict => "Conflict",
                _ => "Server Failure"
            };

        static string GetType(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section—6.5.1",
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section—6.5.4",
                ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section—6.5.8",
                _ => "https://tools.ietf.org/html/rfc7231#section—6.6.1"
            };
    }
}

