using Auction.Api.Dtos;
using Auction.Domain.Common;
using Auction.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Auction.Api.Middlewares;

public class ExceptionalMiddleware
{
    private readonly ILogger< ExceptionalMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionalMiddleware(
        RequestDelegate next,
        ILogger< ExceptionalMiddleware> logger
        )
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (BusinessException exception)
        {
            _logger.LogWarning("Exception in middleware message | exception - {Exception}", exception);
            
            var errorDto = new ErrorDto
            {
                Kind = exception.ErrorKind,
                Messages = new List<string>
                {
                    exception.ToString()
                }
            };

            context.Response.StatusCode = errorDto.Kind switch
            {
                ErrorKind.InvalidData => StatusCodes.Status400BadRequest,
                ErrorKind.InvalidOperation => StatusCodes.Status409Conflict,
                ErrorKind.NotFound => StatusCodes.Status404NotFound,
                ErrorKind.PermissionDenied => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            await context.Response.WriteAsJsonAsync(errorDto);
        }
        catch (Exception exception)
        {
            _logger.LogError("Exception in middleware message | exception - {Exception}", exception);
            
            var errorDto = new ErrorDto
            {
                Kind = ErrorKind.Unknown,
                Messages = new List<string>
                {
                    exception.ToString()
                }
            };
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(errorDto);
        }
    }
}