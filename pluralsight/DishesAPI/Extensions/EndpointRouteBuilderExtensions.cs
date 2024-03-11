using DishesAPI.EndpointFilters;
using DishesAPI.Handlers;
using DishesAPI.Models;

namespace DishesAPI.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder RegisterDishesEndpoints(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder dishesEndpoints = app.MapGroup("/dishes")
                .RequireAuthorization();
            RouteGroupBuilder dishesWithGuidEndpoints = dishesEndpoints.MapGroup("/{dishId:guid}")
                .AddEndpointFilter<LogNotFoundResponseFilter>();                
            RouteGroupBuilder dishesWithStringNameEndpoints = dishesEndpoints.MapGroup("/{dishName:alpha}");
            RouteGroupBuilder ingredientsEndpoints = dishesWithGuidEndpoints.MapGroup("/ingredient");
            RouteGroupBuilder dishesWithGuidEndpointsAndProtectedDishes = dishesWithGuidEndpoints
                    .RequireAuthorization("RequireAdminFromKilmarnock")
                    .AddEndpointFilter(new DishIsLockedFilter(new Guid("6684e810-4da2-4588-8895-5a8ace1b127f")));


            dishesEndpoints.MapGet(string.Empty, DishesHandlers.GetDishesAsync);
            dishesWithGuidEndpoints.MapGet(string.Empty,DishesHandlers.GetDishesByGuidAsync)
                .WithName("GetDishByGuid")
                .WithOpenApi()
                .WithSummary("Get a dish by providing an id")
                .WithDescription("Dishes are identified by guids and so you need to provide one in order" +
                "to get one specific dish back.");

            dishesWithStringNameEndpoints.MapGet(string.Empty, DishesHandlers.GetDishesByNameAsync);
            dishesEndpoints.MapPost(string.Empty, DishesHandlers.CreateDishAsync)
                .RequireAuthorization("RequireAdminFromKilmarnock")
                .AddEndpointFilter<ValidateAnnotationsFilter<DishForCreationDTO>>()
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .Accepts<DishForCreationDTO>("application/json");
            dishesWithGuidEndpointsAndProtectedDishes.MapPut(string.Empty, DishesHandlers.UpdateDishAsync);
            dishesWithGuidEndpointsAndProtectedDishes.MapDelete(string.Empty, DishesHandlers.DeleteDishAsync);                

            ingredientsEndpoints.MapGet(string.Empty, DishesHandlers.GetDishIngredientsAsync)
                .RequireAuthorization();
            ingredientsEndpoints.MapPost(string.Empty, () => {
                throw new NotImplementedException();
            });

            return app;
        }        
    }
}
