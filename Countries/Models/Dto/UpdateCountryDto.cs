namespace Countries.Models.Dto;

public class UpdateCountryDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? IsoCode { get; set; }
}
