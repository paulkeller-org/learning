using AutoMapper;
using DishesAPI.Entities;
using DishesAPI.Models;

namespace DishesAPI.Profiles
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            // This sets up a relationship 
            CreateMap<Ingredient, IngredientDTO>()
                .ForMember(s => s.DishId, d => d.MapFrom(m => m.Dishes.First().Id));        //
        }
    }
}
