namespace InstitutionPanel.Middlewares
{
    using InstitutionPanel.Models.Common;
    using System.Net;
    using System.Text.Json;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<string>.FailureResponse(
                    "An unexpected error occurred",
                    new List<string> { ex.Message });

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }

}
