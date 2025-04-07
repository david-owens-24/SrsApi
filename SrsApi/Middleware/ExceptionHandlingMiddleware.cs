using SrsApi.Classes.ApiResponses;
using SrsApi.Interfaces;
using System;
using System.Net;

namespace SrsApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IErrorService errorService)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unhandled exception has occurred.");                

                // Return error in standard ApiResponse
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new SrsApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Response = null,
                    ErrorCode = null,
                    ErrorMessage = "An unexpected error occurred."
                };

                // Try to log to db
                try
                {
                    var dbError = await errorService.LogError(ex);

                    response.ErrorCode = dbError.UID.ToString();
                }
                catch
                {
                    // Failed to log to database
                    // TODO: decide what to do here, for now, just throw the original exception
                    throw ex;
                }               

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
