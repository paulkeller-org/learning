using DishesAPI.Handlers;

namespace DishesAPI.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder RegisterDishesEndpoints(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder dishesEndpoints = app.MapGroup("/dishes");
            RouteGroupBuilder dishesWithGuidEndpoints = dishesEndpoints.MapGroup("/{dishId:guid}");
            RouteGroupBuilder dishesWithStringNameEndpoints = dishesEndpoints.MapGroup("/{dishName:alpha}");
            RouteGroupBuilder ingredientsEndpoints = dishesWithGuidEndpoints.MapGroup("/ingredient");

            dishesEndpoints.MapGet(string.Empty, DishesHandlers.GetDishesAsync);

            dishesWithGuidEndpoints.MapGet(string.Empty,DishesHandlers.GetDishesByGuidAsync)
                .WithName("GetDishByGuid");

            dishesWithStringNameEndpoints.MapGet(string.Empty, DishesHandlers.GetDishesByNameAsync);

            dishesEndpoints.MapPost(string.Empty, DishesHandlers.CreateDishAsync);

            dishesWithGuidEndpoints.MapPut(string.Empty, DishesHandlers.UpdateDishAsync);

            dishesWithGuidEndpoints.MapDelete(string.Empty, DishesHandlers.DeleteDishAsync);

            ingredientsEndpoints.MapGet(string.Empty, DishesHandlers.GetDishIngredientsAsync);

            ingredientsEndpoints.MapPost(string.Empty, () => {
                throw new NotImplementedException();
            });
            return app;
        }        
    }
}
