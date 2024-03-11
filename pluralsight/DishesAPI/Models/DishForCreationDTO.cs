using System.ComponentModel.DataAnnotations;

namespace DishesAPI.Models
{
    public class DishForCreationDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }
    }
}
