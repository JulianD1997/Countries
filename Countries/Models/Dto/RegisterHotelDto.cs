using System.ComponentModel.DataAnnotations;

namespace Countries.Models.Dto;

public class RegisterHotelDto
{
    [Required]
    public string Name { get; set; }
    public string Starts { get; set; }
}
