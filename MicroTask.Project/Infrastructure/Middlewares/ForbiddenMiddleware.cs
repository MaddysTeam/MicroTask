using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{

    /// <summary>
    /// Forbidden middleware invoked if application offline 
    /// or application upgrading,serious bug fixing
    /// </summary>
    public class ForbiddenMiddleware
    {

        public ForbiddenMiddleware(RequestDelegate next, IConfiguration configuration, ILogger logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // step 1. check forbidden in appsettings
            // step 2. check in request url

            bool isForbidden = false;
            Boolean.TryParse(_configuration.GetSection("application:isForbidden").Value, out isForbidden);
            _logger.LogCritical("enter forbidden middleware!");

            if (isForbidden)
            {
                await SendErrorMessage(context);
                _logger.LogCritical("this application is forbidden !");
                return;
            }

            var requestKeys = context.Request.Query.Keys;
            if (requestKeys.Any(k => k.ToLower() == "forbidden"))
            {
                await SendErrorMessage(context);
                _logger.LogCritical("this application is forbidden !");
                return;
            }

            await _next.Invoke(context);
        }

        private async Task SendErrorMessage(HttpContext context)
        {
             await context.Response.SendServerErrorResponse("text/plain", "Failed due to FailingMiddleware forbidden");
        }

        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

    }


    /// <summary>
    /// use forbidden middleware by application builder
    /// </summary>
    public static class ForbiddenMiddleareAppBuilderExtension
    {
        public static IApplicationBuilder UseForbiddenMiddleware(this IApplicationBuilder builder,IConfiguration configuration, ILogger<Exception> logger)
        {
           return builder.UseMiddleware<ForbiddenMiddleware>(configuration,logger);
        }
    }

}
