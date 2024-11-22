using System.Net;
using Domain.Common;
using Newtonsoft.Json;

namespace API.Middlewares
{
    public class ControllerMiddleware(RequestDelegate next, ILogger<ControllerMiddleware> logger)
    {
        private static readonly bool IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ArgumentException ex)
            {
                await HandleArgumentException(context, ex);
            }
            catch
            {
                throw;
            }
        }

        private async Task HandleArgumentException(HttpContext context, ArgumentException exception)
        {
            var result = new GenericResponse<object>();
            result.AddError(exception.Message);
            UpdateContext(context, HttpStatusCode.BadRequest);
            var stringResponse = JsonConvert.SerializeObject(result);
            await context.Response.WriteAsync(stringResponse);
        }
        private static void UpdateContext(HttpContext context, HttpStatusCode code)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
        }
    }
}
