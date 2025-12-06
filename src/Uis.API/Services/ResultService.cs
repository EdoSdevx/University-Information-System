using System;
using System.Collections.Generic;
using System.Linq;
using Uis.API.Constants;

namespace Uis.API.Services;


public class ValidationFailure
{
    public string PropertyName { get; set; }
    public string Message { get; set; }

    public ValidationFailure(string propertyName, string message)
    {
        PropertyName = propertyName;
        Message = message;
    }
}

public class ResultService
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }

    public ResultErrorCode? ErrorCode { get; set; }
    public List<ValidationFailure> Errors { get; set; } = new();

    public static ResultService Ok(string? message = null) =>
        new()
        {
            Success = true,
            Message = message ?? "Operation successful",
            StatusCode = 200
        };

    public static ResultService Fail(string message, ResultErrorCode errorCode = ResultErrorCode.ValidationError) =>
        new()
        {
            Success = false,
            Message = message,
            StatusCode = 400,
            ErrorCode = errorCode
        };

    public static ResultService NotFound(string message = "Resource not found") =>
        new()
        {
            Success = false,
            Message = message,
            StatusCode = 404,
            ErrorCode = ResultErrorCode.NotFound
        };

    public static ResultService Unauthorized(string message = "Unauthorized") =>
        new()
        {
            Success = false,
            Message = message,
            StatusCode = 401,
            ErrorCode = ResultErrorCode.Unauthorized
        };

    public static ResultService Forbidden(string message = "Forbidden") =>
        new()
        {
            Success = false,
            Message = message,
            StatusCode = 403,
            ErrorCode = ResultErrorCode.Forbidden
        };

    public static ResultService Conflict(string message) =>
        new()
        {
            Success = false,
            Message = message,
            StatusCode = 409,
            ErrorCode = ResultErrorCode.Conflict
        };

    public static ResultService TooManyRequests(string message = "Too many requests") =>
        new()
        {
            Success = false,
            Message = message,
            StatusCode = 429,
            ErrorCode = ResultErrorCode.TooManyRequests
        };

    public static ResultService ValidationError(List<ValidationFailure> failures) =>
        new()
        {
            Success = false,
            Message = "Validation failed",
            StatusCode = 400,
            ErrorCode = ResultErrorCode.ValidationError,
            Errors = failures
        };

    public static ResultService EnrollmentConflict(string message) =>
        new()
        {
            Success = false,
            Message = message,
            StatusCode = 409,
            ErrorCode = ResultErrorCode.CourseFull
        };
}

public class ResultService<T> : ResultService
{
    public T? Data { get; set; }

    public static ResultService<T> Ok(T data, string? message = null) =>
        new()
        {
            Success = true,
            Data = data,
            Message = message ?? "Operation successful",
            StatusCode = 200
        };

    public static new ResultService<T> Fail(string message, ResultErrorCode errorCode = ResultErrorCode.ValidationError) =>
        new()
        {
            Success = false,
            Message = message,
            StatusCode = 400,
            ErrorCode = errorCode
        };

    public static new ResultService<T> NotFound(string message = "Resource not found") =>
        new()
        {
            Success = false,
            Message = message,
            StatusCode = 404,
            ErrorCode = ResultErrorCode.NotFound
        };
}

public class PagedResultService<T> : ResultService<IEnumerable<T>>
{

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static PagedResultService<T> Ok(IEnumerable<T> data, int pageIndex, int pageSize, int totalCount, string? message = null) =>
        new()
        {
            Success = true,
            Data = data,
            Message = message ?? "Operation successful",
            StatusCode = 200,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalCount
        };

    public static PagedResultService<T> Fail(string message) =>
        new()
        {
            Success = false,
            Message = message,
            StatusCode = 400,
            Data = Enumerable.Empty<T>()
        };
}