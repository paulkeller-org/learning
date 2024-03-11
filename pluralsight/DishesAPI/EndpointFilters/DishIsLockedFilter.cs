
using System.Net;

namespace DishesAPI.EndpointFilters
{
    // An endpoint filter is very good for implementing business logic. 

    public class DishIsLockedFilter(Guid dishToLock) : IEndpointFilter
    {
        private readonly Guid dishToLock = dishToLock;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var dishId = context.GetArgument<Guid>(0);
            if (dishId == dishToLock)
            {
                return TypedResults.Problem(new()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "This dish cannot be updated, sorry!",
                    Detail = "This dish cannot be updated or deleted in the system."
                });
            }

            // Be careful when working in here as a step further down the 
            // pipeline may short circuit the response 

            return await next.Invoke(context);
        }
    }
}
