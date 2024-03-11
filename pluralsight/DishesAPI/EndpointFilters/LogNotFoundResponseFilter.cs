
using System.Net;

namespace DishesAPI.EndpointFilters
{
    public class LogNotFoundResponseFilter : IEndpointFilter
    {
        private readonly ILogger<LogNotFoundResponseFilter> logger;

        public LogNotFoundResponseFilter(ILogger<LogNotFoundResponseFilter> logger)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            this.logger = logger;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var result = await next(context);

            // This is used to validate a response and is hit after the request is 
            // serviced.
            if ((result is INestedHttpResult) && (result as INestedHttpResult)?.Result is IStatusCodeHttpResult { StatusCode: (int)HttpStatusCode.NotFound })
            {
                logger.LogInformation($"Resource {context.HttpContext.Request.Path} was not found");
            }
            return result;
        }
    }
}
