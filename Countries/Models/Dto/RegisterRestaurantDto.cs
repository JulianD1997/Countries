using System.ComponentModel.DataAnnotations;

namespace Countries.Models.Dto;

public class RegisterRestaurantDto
{
    [Required]
    public string Name { get; set; }
    public string Type { get; set; }
}
