using AutoMapper;
using DishesAPI.DbContexts;
using DishesAPI.Entities;
using DishesAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DishesAPI.Handlers
{
    public static class DishesHandlers
    {
        public static async Task<Ok<IEnumerable<DishDTO>>> GetDishesAsync(DishesDbContext dishesDbContext, IMapper mapper, ILogger<DishDTO> logger, string? name) {

            logger.LogInformation("Getting the dishes.");
            var dishes = await dishesDbContext.Dishes
                .Where(d => string.IsNullOrEmpty(name) || d.Name.Contains(name)).ToListAsync();
            return TypedResults.Ok(mapper.Map<IEnumerable<DishDTO>>(dishes));
        } 

        public static async Task<Results<NotFound, Ok<DishDTO>>> GetDishesByGuidAsync(Guid dishId, DishesDbContext dishesDbContext, IMapper mapper) {
            var dish = await dishesDbContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == dishId);

            if (dish is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(mapper.Map<DishDTO>(dish));
        }

        public static async Task<Results<NotFound, Ok<DishDTO>>> GetDishesByNameAsync(string dishName, DishesDbContext dishesDbContext, IMapper mapper) {
                var dish = await dishesDbContext.Dishes.FirstOrDefaultAsync(dish => dish.Name == dishName);
                if (dish is null)
                {
                    return TypedResults.NotFound();
                }
                return TypedResults.Ok(mapper.Map<DishDTO>(dish));
        }

        public static async Task<Ok<IEnumerable<IngredientDTO>>> GetDishIngredientsAsync(Guid dishId, DishesDbContext dishesDbContext, IMapper mapper) {
                var ingredients = (await(dishesDbContext.Dishes
                    .Include(i => i.Ingredients)
                    .FirstOrDefaultAsync(dish => dish.Id == dishId)))?.Ingredients ?? [];

                return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDTO>>(ingredients));
            }

        public static async Task<CreatedAtRoute<DishDTO>> CreateDishAsync(DishForCreationDTO dishForCreationDTO, DishesDbContext dishesDbContext,
                    IMapper mapper) {

            Dish newDish = mapper.Map<Dish>(dishForCreationDTO);
            dishesDbContext.Add(newDish);
            await dishesDbContext.SaveChangesAsync();
            DishDTO dishToReturn = mapper.Map<DishDTO>(newDish);
            return TypedResults.CreatedAtRoute(dishToReturn, "GetDishByGuid", new { dishId = newDish.Id });

            //// Note: That the new object at the end is the parameter passed into the get method 
            //string? newDishLink = linkGenerator.GetUriByName(httpContext, "GetDishByGuid", new { dishId = newDish.Id });        
            //return TypedResults.Created(newDishLink, dishToReturn);
        }

        public static async Task<Results<NotFound, NoContent>> UpdateDishAsync(
            Guid dishId,
            DishForUpdateDTO dishForUpdateDTO,
            DishesDbContext dishesDbContext,
            IMapper mapper) {

                var dishToUpdate = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

                if (dishToUpdate == null)
                {
                    return TypedResults.NotFound();
                }

                mapper.Map(dishForUpdateDTO, dishToUpdate);
                await dishesDbContext.SaveChangesAsync();

                return TypedResults.NoContent();
        }

        public static async Task<Results<NotFound, NoContent>> DeleteDishAsync(
                Guid dishId,
                DishesDbContext dishesDbContext) {

                var dishToDelete = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

                if (dishToDelete == null)
                {
                    return TypedResults.NotFound();
                }

                dishesDbContext.Dishes.Remove(dishToDelete);
                await dishesDbContext.SaveChangesAsync();

                return TypedResults.NoContent();
        }
    }
}
