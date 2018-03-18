﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure
{

    public class ExcepitonFilter : IExceptionFilter
    {

        private readonly ILogger<ExcepitonFilter> logger;

        public ExcepitonFilter(ILogger<ExcepitonFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult), context.Exception.Message);

            var errorMessage = new ErrorMessage
            {
                Messages = new[] { context.Exception.Message }
            };

            if (context.Exception.GetType() == typeof(TasktException))
            {
                context.Result = new BadRequestObjectResult(errorMessage);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                context.Result = new InternalServerErrorResult(errorMessage);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;
        }   

        private class ErrorMessage
        {
            public string[] Messages { get; set; }
        }

    }

}
