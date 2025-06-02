using System.Text.Json;

namespace Vehicle
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            var response = new ErrorResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "An unexpected error occurred. Please try again later."
            };

            switch (exception)
            {
                case NotFoundException notFound:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Message = notFound.Message;
                    break;
                case ValidationException validation:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = validation.Message;
                    response.Errors = validation.Errors;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}