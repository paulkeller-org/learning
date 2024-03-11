using MiniValidation;

namespace DishesAPI.EndpointFilters
{
    // This is used to validate an incoming DTO object 
    public class ValidateAnnotationsFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var dtoToValidate = context.Arguments.FirstOrDefault(arg => arg is T);            

            if (!MiniValidator.TryValidate(dtoToValidate, out var errors))
            {
                return TypedResults.ValidationProblem(errors);
            }

            return await next(context);
        }
    }
}
