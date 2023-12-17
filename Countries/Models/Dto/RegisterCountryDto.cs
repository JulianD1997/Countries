using System.ComponentModel.DataAnnotations;

namespace Countries.Models.Dto;

public class RegisterCountryDto
{
    [Required]
    public string Name { get; set; }
    public string IsoCode { get; set; }
}
