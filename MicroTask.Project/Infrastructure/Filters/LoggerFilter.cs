using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public class ActionLoggerFilterAttribute : TypeFilterAttribute
    {

        public ActionLoggerFilterAttribute() : base(typeof(ActionLoggerFilterImp))
        {

        }

        private class ActionLoggerFilterImp : IActionFilter
        {

            public ActionLoggerFilterImp(ILoggerFactory factory)
            {
                logger = factory.CreateLogger<ActionLoggerFilterImp>();
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                logger.LogTrace($"action name: {context.ActionDescriptor.DisplayName} executing!");

            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                logger.LogTrace($"action name: {context.ActionDescriptor.DisplayName} executed!");

            }


            private readonly ILogger logger;

        }

    }

}
